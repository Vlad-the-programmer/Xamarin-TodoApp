using System.Threading.Tasks;

namespace Todo.Helpers.CustomerSSLBypass
{
    using System.Net.Http;
    using System.Threading;

    public class BypassSslValidationHandler : DelegatingHandler
    {
        public BypassSslValidationHandler()
        {
            // Configure the inner handler to bypass SSL validation
            InnerHandler = new HttpClientHandler
            {
                //ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, certChain, policyErrors) => true
            };
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Forward the request to the inner handler
            return base.SendAsync(request, cancellationToken);
        }
    }
}
