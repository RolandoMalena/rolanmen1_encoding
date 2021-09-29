using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace WorkflowRunner.ExtensionMethods
{
    public static class HttpClientFactoryExtensionMethods
    {
        public static Task<HttpResponseMessage> MakeRequest(this HttpClientFactoryLite.IHttpClientFactory factory, HttpMethod method, string url, string authToken, CancellationToken cancellationToken = default)
        {
            return factory.MakeRequest(method, url, authToken, null, cancellationToken);
        }

        public static Task<T> MakeRequest<T>(this HttpClientFactoryLite.IHttpClientFactory factory, HttpMethod method, string url, string authToken, CancellationToken cancellationToken = default)
        {
            return factory.MakeRequest<T>(method, url, authToken, null, cancellationToken);
        }

        public static async Task<HttpResponseMessage> MakeRequest(this HttpClientFactoryLite.IHttpClientFactory factory, HttpMethod method, string url, string authToken, object requestBody, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(method, url);
            request.Headers.Add("Authorization", $"Bearer {authToken}");
            request.Headers.Add("User-Agent", "WorkflowRunnerApp");

            if (requestBody != null)
            {
                var serializedBody = JsonSerializer.Serialize(requestBody);
                request.Content = new StringContent(serializedBody, Encoding.UTF8, "application/json");
            }

            var client = factory.CreateClient(Guid.NewGuid().ToString());
            var response = await client.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new HttpResponseException(response);

            return response;
        }

        public static async Task<T> MakeRequest<T>(this HttpClientFactoryLite.IHttpClientFactory factory, HttpMethod method, string url, string authToken, object requestBody, CancellationToken cancellationToken = default)
        {
            var response = await factory.MakeRequest(method, url, authToken, requestBody, cancellationToken);

            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(body);
        }
    }
}
