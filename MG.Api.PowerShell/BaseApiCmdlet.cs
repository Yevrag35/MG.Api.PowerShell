using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MG.Api.PowerShell
{
    public abstract class BaseApiCmdlet : PSCmdlet
    {
        #region FIELDS/CONSTANTS
        private const string CONTENT_TYPE = "application/json";
        private const string DEBUG_API_MSG = "Sending {0} request to: {1}{2}";
        private const string DEBUG_API_AND_BODY_MSG = DEBUG_API_MSG + "{3}{3}REQUEST BODY:{4}";

        #endregion

        #region PROPERTIES
        protected abstract string BaseUri { get; }
        protected abstract Uri BaseUrl { get; }
        public HttpClient HttpClient { get; protected set; }

        #endregion

        #region PARAMETERS


        #endregion

        #region CMDLET PROCESSING

        #endregion

        #region BACKEND METHODS
        protected string TryGet(string endpoint)
        {
            this.WriteApiDebug(endpoint, HttpMethod.Get, out string apiPath);

            try
            {
                Task<HttpResponseMessage> task = this.HttpClient.GetAsync(apiPath, HttpCompletionOption.ResponseContentRead);
                task.Wait();

                string result = null;
                using (var response = task.Result.EnsureSuccessStatusCode())
                {
                    using (var content = response.Content)
                    {
                        Task<string> strTask = content.ReadAsStringAsync();
                        strTask.Wait();
                        result = strTask.Result;
                    }
                }
            }
            catch (HttpRequestException hre)
            {

            }
        }

        #endregion

        #region LOGGING METHODS
        protected virtual void WriteApiDebug(string endpoint, HttpMethod method, out string apiPath)
        {
            apiPath = this.BaseUri + endpoint;

            string msg = string.Format(
                DEBUG_API_MSG,
                method.Method,
                this.BaseUrl,
                apiPath
            );

            base.WriteDebug(msg);
        }

        protected virtual void WriteApiDebug(string endpoint, HttpMethod method, string body, out string apiPath)
        {
            apiPath = this.BaseUri + endpoint;

            string msg = string.Format(
                DEBUG_API_AND_BODY_MSG,
                method.Method,
                this.BaseUrl,
                apiPath,
                Environment.NewLine,
                body
            );

            base.WriteDebug(msg);
        }

        #endregion

        #region EXCEPTION METHODS
        /// <summary>
        /// Takes in an <see cref="Exception"/> and returns the innermost <see cref="Exception"/> as a result.
        /// </summary>
        /// <param name="e">The exception to pull the innermost <see cref="Exception"/> from.</param>
        protected virtual Exception GetAbsoluteException(Exception e)
        {
            while (e.InnerException != null)
            {
                e = e.InnerException;
            }
            return e;
        }

        /// <summary>
        /// Issues a <see cref="PSCmdlet"/>.WriteError from a given string message and <see cref="ErrorCategory"/>.
        /// </summary>
        /// <param name="message">The exception message to be displayed in the <see cref="ErrorRecord"/>.</param>
        /// <param name="category">The category of the error.</param>
        protected void WriteError(string message, ErrorCategory category) =>
            this.WriteError(new ArgumentException(message), category, null);

        /// <summary>
        /// Issues a <see cref="PSCmdlet"/>.WriteError from a given string message, <see cref="ErrorCategory"/>, and Target Object.
        /// </summary>
        /// <param name="message">The exception message to be displayed in the <see cref="ErrorRecord"/>.</param>
        /// <param name="category">The category of the error.</param>
        /// <param name="targetObject">The object used as the 'targetObject' in an <see cref="ErrorRecord"/>.</param>
        protected void WriteError(string message, ErrorCategory category, object targetObject) =>
            this.WriteError(new ArgumentException(message), category, targetObject);

        /// <summary>
        /// /// Issues a <see cref="PSCmdlet"/>.WriteError from a given string message, base <see cref="Exception"/>, <see cref="ErrorCategory"/>, and Target Object.
        /// </summary>
        /// <param name="message">The exception message to be displayed in the <see cref="ErrorRecord"/>.</param>
        /// <param name="exception">The exception whose InnerException will be become the InnerException of the <see cref="ErrorRecord"/> and its type will be used as the FullyQualifiedErrorId.</param>
        /// <param name="category">The category of the error.</param>
        /// <param name="targetObject">The object used as the 'targetObject' in an <see cref="ErrorRecord"/>.</param>
        protected void WriteError(string message, Exception exception, ErrorCategory category, object targetObject)
        {
            exception = this.GetAbsoluteException(exception);

            var errRec = new ErrorRecord(new InvalidOperationException(message, exception), exception.GetType().FullName, category, targetObject);
            base.WriteError(errRec);
        }

        /// <summary>
        /// Issues a <see cref="PSCmdlet"/>.WriteError from a given base exception and <see cref="ErrorCategory"/>.
        /// </summary>
        /// <param name="baseException">The base exception will be become the InnerException of the <see cref="ErrorRecord"/> and its type will be used as the FullyQualifiedErrorId.</param>
        /// <param name="category"></param>
        protected void WriteError(Exception baseException, ErrorCategory category) => this.WriteError(baseException, category, null);

        /// <summary>
        /// Issues a <see cref="PSCmdlet"/>.WriteError from a base <see cref="Exception"/>, <see cref="ErrorCategory"/>, and Target Object.
        /// </summary>
        /// <param name="message">The exception message to be displayed in the <see cref="ErrorRecord"/>.</param>
        /// <param name="baseException">The base exception will be become the InnerException of the <see cref="ErrorRecord"/> and its type will be used as the FullyQualifiedErrorId.</param>
        /// <param name="category">The category of the error.</param>
        /// <param name="targetObject">The object used as the 'targetObject' in an <see cref="ErrorRecord"/>.</param>
        protected void WriteError(Exception baseException, ErrorCategory category, object targetObject)
        {
            var errRec = new ErrorRecord(baseException, baseException.GetType().FullName, category, targetObject);
            base.WriteError(errRec);
        }

        #endregion
    }
}