using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MG.Api.PowerShell
{
    public static class HttpClientExtensions
    {
        internal const string PATCH = "PATCH";

        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent httpContent)
        {
            Uri.TryCreate(requestUri, UriKind.Relative, out Uri real);
            return await PatchAsync(client, real, httpContent);
        }

        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, 
            HttpContent httpContent, HttpCompletionOption onCompletion)
        {
            Uri.TryCreate(requestUri, UriKind.Relative, out Uri real);
            return await PatchAsync(client, real, httpContent, onCompletion);
        }

        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri,
            HttpContent httpContent, HttpCompletionOption onCompletion, CancellationToken cancellationToken)
        {
            Uri.TryCreate(requestUri, UriKind.Relative, out Uri real);
            return await PatchAsync(client, real, httpContent, onCompletion, cancellationToken);
        }

        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent httpContent)
        {
            var method = new HttpMethod(PATCH);
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = httpContent
            };

            HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead);
            return response;
        }

        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, 
            HttpContent httpContent, HttpCompletionOption onCompletion)
        {
            var method = new HttpMethod(PATCH);
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = httpContent
            };

            HttpResponseMessage response = await client.SendAsync(request, onCompletion);
            return response;
        }

        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, 
            HttpContent httpContent, HttpCompletionOption onCompletion, CancellationToken cancellationToken)
        {
            var method = new HttpMethod(PATCH);
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = httpContent
            };

            HttpResponseMessage response = await client.SendAsync(request, onCompletion, cancellationToken);
            return response;
        }
    }
}
