using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Todo.Helpers.Exception;
using Todo.Helpers.ResponseModels;
using Todo.Models;
using Todo.Services;
using Xamarin.Forms;

namespace Todo.ApiServices
{
    public class UsersApiService : IUsersApiService
    {
        private readonly HttpClient _httpClient;
        private readonly Todo.Services.IAuthTokenStore _tokenStore;

        public UsersApiService()
        {
            var httpClientService = DependencyService.Get<IHttpClientService>();
            _httpClient = httpClientService.GetClient();

            _tokenStore = new SecureStorageTokenStore();
        }

        public async Task<User> GetUserAsync(string username, CancellationToken cancellationToken = default)
        {
            ValidateUsername(username);

            try
            {
                var response = await _httpClient.GetAsync(
                    $"{Constants.Constants.UsersEndpoints}/{WebUtility.UrlEncode(username)}",
                    cancellationToken);

                return await HandleResponseAsync<User>(response,
                    whenNotFound: () => null,
                    whenUnauthorized: () => throw new ApiUnauthorizedException("Authentication required"));
            }
            catch (Exception ex) when (!(ex is ApiException))
            {
                throw new ApiException("Failed to retrieve user", ex);
            }
        }

        public async Task<List<Models.Todo>> GetUserTodosAsync(int userId, CancellationToken cancellationToken = default)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            try
            {
                var response = await _httpClient.GetAsync(
                    $"{Constants.Constants.UsersEndpoints}/{userId}/todos",
                    cancellationToken);

                return await HandleResponseAsync<List<Models.Todo>>(response,
                    whenNotFound: () => new List<Models.Todo>(),
                    whenUnauthorized: () => throw new ApiUnauthorizedException("Authentication required"));
            }
            catch (Exception ex) when (!(ex is ApiException))
            {
                throw new ApiException("Failed to retrieve todos", ex);
            }
        }

        public async Task<LoginResult> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
        {
            ValidateUsername(username);
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty", nameof(password));

            try
            {
                var loginData = new { username, password };
                var content = new StringContent(
                    JsonConvert.SerializeObject(loginData),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                    Constants.Constants.UsersLoginEndpoints,
                    content,
                    cancellationToken);

                var result = await HandleResponseAsync<LoginResult>(response,
                    whenUnauthorized: () => throw new ApiUnauthorizedException("Invalid credentials"));

                if (!string.IsNullOrEmpty(result?.Token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", result.Token);

                    _tokenStore.SaveToken(result.Token);
                }

                return result;
            }
            catch (ApiUnauthorizedException)
            {
                throw;
            }
            catch (Exception ex) when (!(ex is ApiException))
            {
                throw new ApiException("Login failed", ex);
            }
        }

        public async Task<RegistrationResult> RegisterAsync(User user, CancellationToken cancellationToken = default)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            ValidateInputs(user);

            try
            {
                string json;
                try
                {
                    json = JsonConvert.SerializeObject(user);
                    Debug.WriteLine($"Registration payload: {json}");
                }
                catch (JsonException jsonEx)
                {
                    throw new ApiException("Invalid user data format", jsonEx);
                }

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Debug.WriteLine($"Sending registration to: {_httpClient.BaseAddress}{Constants.Constants.UsersRegisterEndpoints}");

                var response = await _httpClient.PostAsync(
                    Constants.Constants.UsersRegisterEndpoints,
                    content,
                    cancellationToken);

                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Registration response: {response.StatusCode} - {responseContent}");

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Conflict:
                        throw new ApiConflictException("Username already exists");
                    case HttpStatusCode.BadRequest:
                        try
                        {
                            var errorDetails = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
                            throw new ApiException(errorDetails?.Message ?? "Invalid registration data");
                        }
                        catch
                        {
                            throw new ApiException("Invalid registration data");
                        }
                    default:
                        response.EnsureSuccessStatusCode();
                        return JsonConvert.DeserializeObject<RegistrationResult>(responseContent);
                }
            }
            catch (HttpRequestException httpEx)
            {
                throw new ApiException("Network error during registration", httpEx);
            }
            catch (Exception ex) when (!(ex is ApiException))
            {
                throw new ApiException("Registration failed", ex);
            }
        }

        private async Task<T> HandleResponseAsync<T>(
            HttpResponseMessage response,
            Func<T> whenNotFound = null,
            Func<T> whenUnauthorized = null,
            Func<T> whenConflict = null)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound when whenNotFound != null:
                    return whenNotFound();

                case HttpStatusCode.Unauthorized when whenUnauthorized != null:
                    return whenUnauthorized();

                case HttpStatusCode.Conflict when whenConflict != null:
                    return whenConflict();

                default:
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(content);
            }
        }

        private void ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));
        }

        private bool ValidateInputs(Models.User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                Application.Current.MainPage.DisplayAlert("Error", "All fields are required!", "OK");
                return false;
            }

            if (!Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                Application.Current.MainPage.DisplayAlert("Error", "Invalid email format!", "OK");
                return false;
            }

            if (user.Password.Length < 6)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Password must be at least 6 characters!", "OK");
                return false;
            }

            return true;
        }

    }
}