using System;
using System.Management.Automation;
using System.Net;
using System.Net.Http;

namespace MG.Api.PowerShell
{
    public class ApiPatchRequestException : BaseApiException
    {
        public override string Method => HttpClientExtensions.PATCH;

        public ApiPatchRequestException(string uri, HttpStatusCode? code, Exception e)
            : base(new Uri(uri, UriKind.RelativeOrAbsolute), e)
        {
            if (code.HasValue)
                base.StatusCode = code.Value;
        }

        public ApiPatchRequestException(Uri url, HttpStatusCode? code, Exception e)
            : base(url, e)
        {
            if (code.HasValue)
                base.StatusCode = code.Value;
        }

        public ApiPatchRequestException(string message, Uri url, HttpStatusCode? code, Exception e)
            : base(message, url, e)
        {
            if (code.HasValue)
                base.StatusCode = code.Value;
        }
    }
}
