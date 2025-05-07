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
            var handler = new HttpClientHandler();

            //var bypassHandler = new BypassSslValidationHandler
            //{
            //    InnerHandler = handler
            //};

            //_client = new HttpClient(handler)
            //{
            //    BaseAddress = new Uri(Constants.Constants.BASE_URL),
            //    Timeout = TimeSpan.FromSeconds(120)
            //};


            var client = new HttpClient(handler);
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
