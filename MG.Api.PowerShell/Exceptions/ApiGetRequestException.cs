using System;
using System.Management.Automation;
using System.Net;
using System.Net.Http;

namespace MG.Api.PowerShell
{
    public class ApiGetRequestException : BaseApiException
    {
        public override string Method => HttpMethod.Get.Method;

        public ApiGetRequestException(string uri, HttpStatusCode? code, Exception e)
            : base(new Uri(uri, UriKind.RelativeOrAbsolute), e)
        {
            if (code.HasValue)
                base.StatusCode = code.Value;
        }

        public ApiGetRequestException(Uri url, HttpStatusCode? code, Exception e)
            : base(url, e)
        {
            if (code.HasValue)
                base.StatusCode = code.Value;
        }

        public ApiGetRequestException(string message, string url, HttpStatusCode? code, Exception e)
            : base(message, url, e)
        {
            if (code.HasValue)
                this.StatusCode = code.Value;
        }

        public ApiGetRequestException(string message, Uri url, HttpStatusCode? code, Exception e)
            : base(message, url, e)
        {
            if (code.HasValue)
                base.StatusCode = code.Value;
        }
    }
}
