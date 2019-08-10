using System;
using System.Management.Automation;
using System.Net;
using System.Net.Http;

namespace MG.Api.PowerShell
{
    public abstract class BaseApiException : HttpRequestException
    {
        #region FIELDS/CONSTANTS
        private const string BASE_MSG = "An API request did not execute successfully.";
        private const string HTTP_CODE_MSG = "The HttpResponse code was {0}.";

        #endregion

        #region PROPERTIES
        public abstract string Method { get; }
        public virtual HttpStatusCode StatusCode { get; protected set; }
        public virtual Uri Url { get; protected set; }

        #endregion

        #region CONSTRUCTORS
        public BaseApiException() : base(BASE_MSG) { }

        public BaseApiException(string message) : base(message) { }

        public BaseApiException(HttpStatusCode responseCode)
            : base(string.Format(HTTP_CODE_MSG, responseCode)) => this.StatusCode = responseCode;

        public BaseApiException(Uri url) : this(BASE_MSG) { }

        public BaseApiException(string message, Uri url)
            : base(message) => this.Url = url;

        public BaseApiException(string message, Uri url, Exception e)
            : base(message, e) => this.Url = url;

        public BaseApiException(Uri url, Exception e)
            : base(BASE_MSG, e) => this.Url = url;

        public BaseApiException(Uri url, HttpStatusCode code, Exception e)
            : base(BASE_MSG + "  " + string.Format(HTTP_CODE_MSG, (int)code), e)
        {
            this.StatusCode = code;
            this.Url = url;
        }

        public BaseApiException(string message, Uri url, HttpStatusCode code)
            : this(message, url, code, null) { }

        public BaseApiException(string message, Uri url, HttpStatusCode code, Exception exception)
            : base(message + "  " + string.Format(HTTP_CODE_MSG, (int)code), exception)
        {
            this.StatusCode = code;
            this.Url = url;
        }

        #endregion
    }
}