using System.Net.Http;

namespace Todo.Services
{
    public interface IHttpClientService
    {
        HttpClient GetClient();
    }
}
