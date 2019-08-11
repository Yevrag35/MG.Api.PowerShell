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
using System.Threading;
using System.Threading.Tasks;

namespace MG.Api.PowerShell
{
    public abstract class BaseApiCmdlet : PSCmdlet
    {
        #region FIELDS/CONSTANTS
        private const string CONTENT_TYPE = "application/json";
        private const string DEBUG_API_MSG = "Sending {0} request to: {1}{2}";
        private const string DEBUG_API_AND_BODY_MSG = DEBUG_API_MSG + "{3}{3}REQUEST BODY:{4}";
        private const string TIMED_OUT = "No response was received within the time alloted.  The request was cancelled.";

        #endregion

        #region PROPERTIES
        protected abstract string BaseUri { get; }
        protected abstract Uri BaseUrl { get; }
        protected abstract HttpClient HttpClient { get; }

        #endregion

        #region CMDLET PROCESSING
        protected override void BeginProcessing() => this.VerifyIsReady();

        #endregion

        protected virtual bool IsNotReady() => this.HttpClient == null;
        protected virtual void VerifyIsReady()
        {
            if (this.IsNotReady())
            {
                throw new CmdletNotReadyException();
            }
        }

        #region API METHODS

        #region GET METHODS
        protected virtual string TryGet(string endpoint)
        {
            this.WriteApiDebug(endpoint, HttpMethod.Get, out string apiPath);
            HttpStatusCode? code = null;
            try
            {
                Task<HttpResponseMessage> task = this.HttpClient.GetAsync(apiPath, HttpCompletionOption.ResponseContentRead);
                task.Wait();

                code = task.Result.StatusCode;
                string result = null;
                using (HttpResponseMessage response = task.Result.EnsureSuccessStatusCode())
                {
                    using (var content = response.Content)
                    {
                        Task<string> strTask = content.ReadAsStringAsync();
                        strTask.Wait();
                        result = strTask.Result;
                    }
                }
                return result;
            }
            catch (HttpRequestException hre)
            {
                this.WriteError(new ApiGetRequestException(apiPath, code, hre), ErrorCategory.InvalidResult);
                return null;
            }
        }

        protected virtual string TryGet(string endpoint, CancellationToken cancelToken)
        {
            this.WriteApiDebug(endpoint, HttpMethod.Get, out string apiPath);
            HttpStatusCode? code = null;
            try
            {
                Task<HttpResponseMessage> task = this.HttpClient.GetAsync(apiPath, HttpCompletionOption.ResponseContentRead, cancelToken);
                task.Wait();

                code = task.Result.StatusCode;
                string result = null;
                using (HttpResponseMessage response = task.Result.EnsureSuccessStatusCode())
                {
                    using (var content = response.Content)
                    {
                        Task<string> strTask = content.ReadAsStringAsync();
                        strTask.Wait();
                        result = strTask.Result;
                    }
                }
                return result;
            }
            catch (TaskCanceledException)
            {
                base.WriteWarning(TIMED_OUT);
                return null;
            }
            catch (HttpRequestException hre)
            {
                this.WriteError(new ApiGetRequestException(apiPath, code, hre), ErrorCategory.InvalidResult);
                return null;
            }
        }

        #endregion

        #region POST METHODS
        protected virtual string TryPost(string endpoint, string postBody)
        {
            this.WriteApiDebug(endpoint, HttpMethod.Post, out string apiPath);
            HttpStatusCode? code = null;
            StringContent stringContent = this.BodyToContent(postBody);

            try
            {
                Task<HttpResponseMessage> task = this.HttpClient.PostAsync(apiPath, stringContent);
                task.Wait();

                code = task.Result.StatusCode;
                string result = null;
                using (HttpResponseMessage response = task.Result.EnsureSuccessStatusCode())
                {
                    using (var content = response.Content)
                    {
                        Task<string> strTask = content.ReadAsStringAsync();
                        strTask.Wait();
                        result = strTask.Result;
                    }
                }
                return result;
            }
            catch (HttpRequestException hre)
            {
                this.WriteError(new ApiPostRequestException(apiPath, code, hre), ErrorCategory.InvalidResult);
                return null;
            }
        }

        protected virtual string TryPost(string endpoint, string postBody, CancellationToken cancelToken)
        {
            this.WriteApiDebug(endpoint, HttpMethod.Post, out string apiPath);
            HttpStatusCode? code = null;
            StringContent stringContent = this.BodyToContent(postBody);

            try
            {
                Task<HttpResponseMessage> task = this.HttpClient.PostAsync(apiPath, stringContent, cancelToken);
                task.Wait();

                code = task.Result.StatusCode;
                string result = null;
                using (HttpResponseMessage response = task.Result.EnsureSuccessStatusCode())
                {
                    using (var content = response.Content)
                    {
                        Task<string> strTask = content.ReadAsStringAsync();
                        strTask.Wait();
                        result = strTask.Result;
                    }
                }
                return result;
            }
            catch (TaskCanceledException)
            {
                base.WriteWarning(TIMED_OUT);
                return null;
            }
            catch (HttpRequestException hre)
            {
                this.WriteError(new ApiPostRequestException(apiPath, code, hre), ErrorCategory.InvalidResult);
                return null;
            }
        }

        #endregion

        #region PUT METHODS
        protected virtual string TryPut(string endpoint, string putBody)
        {
            this.WriteApiDebug(endpoint, HttpMethod.Put, out string apiPath);
            HttpStatusCode? code = null;
            StringContent stringContent = this.BodyToContent(putBody);

            try
            {
                Task<HttpResponseMessage> task = this.HttpClient.PutAsync(apiPath, stringContent);
                task.Wait();

                code = task.Result.StatusCode;
                string result = null;
                using (HttpResponseMessage response = task.Result.EnsureSuccessStatusCode())
                {
                    using (var content = response.Content)
                    {
                        Task<string> strTask = content.ReadAsStringAsync();
                        strTask.Wait();
                        result = strTask.Result;
                    }
                }
                return result;
            }
            catch (HttpRequestException hre)
            {
                this.WriteError(new ApiPutRequestException(apiPath, code, hre), ErrorCategory.InvalidResult);
                return null;
            }
        }

        protected virtual string TryPut(string endpoint, string putBody, CancellationToken cancelToken)
        {
            this.WriteApiDebug(endpoint, HttpMethod.Put, out string apiPath);
            HttpStatusCode? code = null;
            StringContent stringContent = this.BodyToContent(putBody);

            try
            {
                Task<HttpResponseMessage> task = this.HttpClient.PutAsync(apiPath, stringContent, cancelToken);
                task.Wait();

                code = task.Result.StatusCode;
                string result = null;
                using (HttpResponseMessage response = task.Result.EnsureSuccessStatusCode())
                {
                    using (var content = response.Content)
                    {
                        Task<string> strTask = content.ReadAsStringAsync();
                        strTask.Wait();
                        result = strTask.Result;
                    }
                }
                return result;
            }
            catch (TaskCanceledException)
            {
                base.WriteWarning(TIMED_OUT);
                return null;
            }
            catch (HttpRequestException hre)
            {
                this.WriteError(new ApiPutRequestException(apiPath, code, hre), ErrorCategory.InvalidResult);
                return null;
            }
        }

        #endregion

        #region PATCH METHODS
        protected virtual string TryPatch(string endpoint, string patchBody)
        {
            this.WriteApiDebug(endpoint, HttpClientExtensions.PATCH, out string apiPath);
            HttpStatusCode? code = null;
            StringContent stringContent = this.BodyToContent(patchBody);

            try
            {
                Task<HttpResponseMessage> task = this.HttpClient.PatchAsync(apiPath,
                    stringContent, HttpCompletionOption.ResponseContentRead);

                task.Wait();

                code = task.Result.StatusCode;
                string result = null;
                using (HttpResponseMessage response = task.Result.EnsureSuccessStatusCode())
                {
                    using (var content = response.Content)
                    {
                        Task<string> strTask = content.ReadAsStringAsync();
                        strTask.Wait();
                        result = strTask.Result;
                    }
                }
                return result;
            }
            catch (HttpRequestException hre)
            {
                this.WriteError(new ApiPatchRequestException(apiPath, code, hre), ErrorCategory.InvalidResult);
                return null;
            }
        }

        protected virtual string TryPatch(string endpoint, string patchBody, CancellationToken cancelToken)
        {
            this.WriteApiDebug(endpoint, HttpClientExtensions.PATCH, out string apiPath);
            HttpStatusCode? code = null;
            StringContent stringContent = this.BodyToContent(patchBody);

            try
            {
                Task<HttpResponseMessage> task = this.HttpClient.PatchAsync(apiPath, 
                    stringContent, HttpCompletionOption.ResponseContentRead, cancelToken);

                task.Wait();

                code = task.Result.StatusCode;
                string result = null;
                using (HttpResponseMessage response = task.Result.EnsureSuccessStatusCode())
                {
                    using (var content = response.Content)
                    {
                        Task<string> strTask = content.ReadAsStringAsync();
                        strTask.Wait();
                        result = strTask.Result;
                    }
                }
                return result;
            }
            catch (TaskCanceledException)
            {
                base.WriteWarning(TIMED_OUT);
                return null;
            }
            catch (HttpRequestException hre)
            {
                this.WriteError(new ApiPatchRequestException(apiPath, code, hre), ErrorCategory.InvalidResult);
                return null;
            }
        }

        #endregion

        #region DELETE METHODS
        protected virtual void TryDelete(string endpoint)
        {
            this.WriteApiDebug(endpoint, HttpMethod.Delete, out string apiPath);
            HttpStatusCode? code = null;
            try
            {
                Task<HttpResponseMessage> task = this.HttpClient.DeleteAsync(apiPath);
                task.Wait();

                code = task.Result.StatusCode;
                task.Result.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException hre)
            {
                this.WriteError(new ApiDeleteRequestException(apiPath, code, hre), ErrorCategory.InvalidResult);
            }
        }

        protected virtual void TryDelete(string endpoint, CancellationToken cancelToken)
        {
            this.WriteApiDebug(endpoint, HttpMethod.Delete, out string apiPath);
            HttpStatusCode? code = null;
            try
            {
                Task<HttpResponseMessage> task = this.HttpClient.DeleteAsync(apiPath, cancelToken);
                task.Wait();

                code = task.Result.StatusCode;
                task.Result.EnsureSuccessStatusCode();
            }
            catch (TaskCanceledException)
            {
                base.WriteWarning(TIMED_OUT);
            }
            catch (HttpRequestException hre)
            {
                this.WriteError(new ApiDeleteRequestException(apiPath, code, hre), ErrorCategory.InvalidResult);
            }
        }

        #endregion

        #endregion

        #region BODY METHODS
        protected virtual StringContent BodyToContent(string postBody) => new StringContent(postBody, Encoding.UTF8, CONTENT_TYPE);

        #endregion

        #region LOGGING METHODS
        protected void WriteApiDebug(string endpoint, HttpMethod method, out string apiPath) =>
            this.WriteApiDebug(endpoint, method.Method, out apiPath);
        protected virtual void WriteApiDebug(string endpoint, string method, out string apiPath)
        {
            apiPath = this.BaseUri + endpoint;

            string msg = string.Format(
                DEBUG_API_MSG,
                method,
                this.BaseUrl,
                apiPath
            );

            base.WriteDebug(msg);
        }
        protected void WriteApiDebug(string endpoint, HttpMethod method, string body, out string apiPath) =>
            this.WriteApiDebug(endpoint, method.Method, body, out apiPath);
        protected virtual void WriteApiDebug(string endpoint, string method, string body, out string apiPath)
        {
            apiPath = this.BaseUri + endpoint;

            string msg = string.Format(
                DEBUG_API_AND_BODY_MSG,
                method,
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