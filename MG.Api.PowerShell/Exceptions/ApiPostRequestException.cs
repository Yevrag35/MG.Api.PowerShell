using System;
using System.Management.Automation;
using System.Net;
using System.Net.Http;

namespace MG.Api.PowerShell
{
    public class ApiPostRequestException : BaseApiException
    {
        public override string Method => HttpMethod.Post.Method;

        public ApiPostRequestException(string uri, HttpStatusCode? code, Exception e)
            : base(new Uri(uri, UriKind.RelativeOrAbsolute), e)
        {
            if (code.HasValue)
                base.StatusCode = code.Value;
        }

        public ApiPostRequestException(Uri url, HttpStatusCode? code, Exception e)
            : base(url, e)
        {
            if (code.HasValue)
                base.StatusCode = code.Value;
        }

        public ApiPostRequestException(string message, Uri url, HttpStatusCode? code, Exception e)
            : base(message, url, e)
        {
            if (code.HasValue)
                base.StatusCode = code.Value;
        }
    }
}
