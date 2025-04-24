using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Todo.Helpers.Exception;
using Todo.Helpers.ResponseModels;
using Todo.Services;
using Xamarin.Forms;

namespace Todo.ApiServices
{
    public class TodoApiService : ITodoApiService
    {
        private readonly HttpClient _httpClient;

        public TodoApiService()
        {
            var httpClientService = DependencyService.Get<IHttpClientService>();
            _httpClient = httpClientService.GetClient();
        }

        public string SearchTerm { get; set; }

        public async Task<bool> AddItemAsync(Models.Todo item)
        {
            try
            {
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(Constants.Constants.TodosEndpoint, content);
                return await HandleSuccessAsBool(response);
            }
            catch (Exception ex) when (!(ex is ApiException))
            {
                throw new ApiException("Failed to add todo", ex);
            }
        }

        public async Task<bool> DeleteItemAsync(Models.Todo item)
        {
            try
            {
                var url = $"{Constants.Constants.TodosEndpoint}/{item.Id}";
                var response = await _httpClient.DeleteAsync(url);
                return await HandleSuccessAsBool(response);
            }
            catch (Exception ex) when (!(ex is ApiException))
            {
                throw new ApiException("Failed to delete todo", ex);
            }
        }

        public async Task<Models.Todo> GetItemAsync(int id)
        {
            try
            {
                var url = $"{Constants.Constants.TodosEndpoint}/{id}";
                var response = await _httpClient.GetAsync(url);

                return await HandleResponseAsync<Models.Todo>(response,
                    whenNotFound: () => null);
            }
            catch (Exception ex) when (!(ex is ApiException))
            {
                throw new ApiException("Failed to retrieve todo item", ex);
            }
        }

        public async Task<IEnumerable<Models.Todo>> GetItemsAsync(bool forceRefresh = false)
        {
            try
            {
                var response = await _httpClient.GetAsync(Constants.Constants.TodosEndpoint);

                return await HandleResponseAsync<List<Models.Todo>>(response,
                    whenNotFound: () => new List<Models.Todo>());
            }
            catch (Exception ex) when (!(ex is ApiException))
            {
                throw new ApiException("Failed to retrieve todos", ex);
            }
        }

        public async Task<List<Models.Todo>> SearchTodosAsync(string searchTerm)
        {
            try
            {
                var url = $"{Constants.Constants.TodosSearchEndpoint}/?term={Uri.EscapeDataString(searchTerm)}";
                var response = await _httpClient.GetAsync(url);

                return await HandleResponseAsync<List<Models.Todo>>(response,
                    whenNotFound: () => new List<Models.Todo>());
            }
            catch (Exception ex) when (!(ex is ApiException))
            {
                throw new ApiException("Failed to search todos", ex);
            }
        }

        public async Task<bool> UpdateItemAsync(Models.Todo item)
        {
            try
            {
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var url = $"{Constants.Constants.TodosEndpoint}/{item.Id}";

                var response = await _httpClient.PutAsync(url, content);
                return await HandleSuccessAsBool(response);
            }
            catch (Exception ex) when (!(ex is ApiException))
            {
                throw new ApiException("Failed to update todo", ex);
            }
        }

        private async Task<T> HandleResponseAsync<T>(
            HttpResponseMessage response,
            Func<T> whenNotFound = null,
            Func<T> whenUnauthorized = null,
            Func<T> whenConflict = null)
        {
            var content = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Response: {response.StatusCode} - {content}");

            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound when whenNotFound != null:
                    return whenNotFound();

                case HttpStatusCode.Unauthorized when whenUnauthorized != null:
                    return whenUnauthorized();

                case HttpStatusCode.Conflict when whenConflict != null:
                    return whenConflict();

                default:
                    if (!response.IsSuccessStatusCode)
                    {
                        try
                        {
                            var errorDetails = JsonConvert.DeserializeObject<ErrorResponse>(content);
                            throw new ApiException(errorDetails?.Message ?? "Unexpected API error");
                        }
                        catch (JsonException)
                        {
                            throw new ApiException($"Unexpected error: {response.StatusCode}");
                        }
                    }

                    return JsonConvert.DeserializeObject<T>(content);
            }
        }

        private async Task<bool> HandleSuccessAsBool(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Response: {response.StatusCode} - {content}");

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var error = JsonConvert.DeserializeObject<ErrorResponse>(content);
                    throw new ApiException(error?.Message ?? "API call failed");
                }
                catch (JsonException)
                {
                    throw new ApiException("API call failed and error could not be parsed.");
                }
            }

            return true;
        }
    }
}
