using System.Threading.Tasks;

namespace Todo.Services
{
    public interface IAuthTokenStore
    {
        void SaveToken(string token);
        Task<string> GetToken();  // Changed to async
        void ClearToken();
    }
}
