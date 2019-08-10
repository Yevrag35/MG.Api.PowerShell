using System;
using System.Management.Automation;
using System.Net;
using System.Net.Http;

namespace MG.Api.PowerShell
{
    public class ApiPutRequestException : BaseApiException
    {
        public override string Method => HttpMethod.Put.Method;

        public ApiPutRequestException(string uri, HttpStatusCode? code, Exception e)
            : base(new Uri(uri, UriKind.RelativeOrAbsolute), e)
        {
            if (code.HasValue)
                base.StatusCode = code.Value;
        }

        public ApiPutRequestException(Uri url, HttpStatusCode? code, Exception e)
            : base(url, e)
        {
            if (code.HasValue)
                base.StatusCode = code.Value;
        }

        public ApiPutRequestException(string message, Uri url, HttpStatusCode? code, Exception e)
            : base(message, url, e)
        {
            if (code.HasValue)
                base.StatusCode = code.Value;
        }
    }
}
