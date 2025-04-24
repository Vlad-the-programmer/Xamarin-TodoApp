using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Todo.Services
{
    public class SecureStorageTokenStore : IAuthTokenStore
    {
        private const string AuthTokenKey = "auth_token";

        public async void SaveToken(string token)
        {
            try
            {
                await SecureStorage.SetAsync(AuthTokenKey, token);
            }
            catch (System.Exception ex)
            {
                // Handle storage exceptions (e.g., when secure storage isn't available)
                System.Diagnostics.Debug.WriteLine($"Failed to save token: {ex.Message}");
                throw;
            }
        }

        public async Task<string> GetToken()
        {
            try
            {
                return await SecureStorage.GetAsync(AuthTokenKey);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to read token: {ex.Message}");
                return string.Empty;
            }
        }

        public void ClearToken()
        {
            try
            {
                SecureStorage.Remove(AuthTokenKey);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to clear token: {ex.Message}");
            }
        }
    }
}