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

        private Uri _uri;

        #endregion

        #region PROPERTIES
        public abstract string Method { get; }
        public virtual HttpStatusCode StatusCode { get; protected set; }
        public virtual string Url
        {
            get => _uri != null
                    ? _uri.ToString()
                    : null;
            protected set
            {
                if (Uri.TryCreate(value, UriKind.Relative, out Uri result))
                    _uri = result;

                else
                    throw new ArgumentException("The specified value is not a valid URI.");
            }
        }

        #endregion

        #region CONSTRUCTORS
        public BaseApiException() : base(BASE_MSG) { }

        public BaseApiException(string message) : base(message) { }

        public BaseApiException(HttpStatusCode responseCode)
            : base(string.Format(HTTP_CODE_MSG, responseCode)) => this.StatusCode = responseCode;

        public BaseApiException(Uri url) : this(BASE_MSG) { }

        public BaseApiException(string message, string url)
            : this(message, url, null) { }

        public BaseApiException(string message, Uri url)
            : base(message) => _uri = url;

        public BaseApiException(string message, Exception e)
            : base(message, e) { }

        public BaseApiException(string message, string url, Exception e)
            : base(message, e)
        {
            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri realUrl))
                _uri = realUrl;
        }

        public BaseApiException(string message, Uri url, Exception e)
            : base(message, e) => _uri = url;

        public BaseApiException(Uri url, Exception e)
            : base(BASE_MSG, e) => _uri = url;

        public BaseApiException(Uri url, HttpStatusCode code, Exception e)
            : base(BASE_MSG + "  " + string.Format(HTTP_CODE_MSG, (int)code), e)
        {
            this.StatusCode = code;
            _uri = url;
        }

        public BaseApiException(string message, Uri url, HttpStatusCode code)
            : this(message, url, code, null) { }

        public BaseApiException(string message, Uri url, HttpStatusCode code, Exception exception)
            : base(message + "  " + string.Format(HTTP_CODE_MSG, (int)code), exception)
        {
            this.StatusCode = code;
            _uri = url;
        }

        #endregion
    }
}