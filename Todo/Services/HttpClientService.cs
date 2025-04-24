using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Todo.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _client;

        public HttpClientService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(Constants.Constants.BASE_URL),
                Timeout = TimeSpan.FromSeconds(120)
            };

            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public HttpClient GetClient() => _client;
    }
}
