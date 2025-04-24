using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Todo.ApiServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(Todo.Services.TodoService))]
namespace Todo.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoApiService _todoApiService;
        public static ObservableCollection<Models.Todo> Todos = new ObservableCollection<Models.Todo>();
        public static IList<Models.Todo> AllTodos = new List<Models.Todo>();
        private static string _searchTerm;

        public string SearchTerm { get { return _searchTerm; } set => _searchTerm = value; }


        public TodoService()
        {
            _todoApiService = new TodoApiService();
        }

        public async Task<IEnumerable<Models.Todo>> GetItemsAsync(bool forceRefresh = false)
        {
            try
            {
                return await _todoApiService.GetItemsAsync(forceRefresh);
            }
            catch (Exception Ex)
            {
                Debug.WriteLine($"Error: {Ex.Message}");
                return new List<Models.Todo>();
            }
        }



        public async Task<bool> AddItemAsync(Models.Todo todo)
        {
            try
            {
                return await _todoApiService.AddItemAsync(todo);
            }
            catch (Exception Ex)
            {
                Debug.WriteLine($"Error: {Ex.Message}");
                return await Task.FromResult(false);
            }
        }


        public async Task<bool> DeleteItemAsync(Models.Todo todo)
        {
            try
            {
                return await _todoApiService.DeleteItemAsync(todo);
            }
            catch (Exception Ex)
            {
                Debug.WriteLine($"Error: {Ex.Message}");
                return await Task.FromResult(false);
            }
        }

        public async Task<bool> UpdateItemAsync(Models.Todo todo)
        {
            try
            {
                return await _todoApiService.UpdateItemAsync(todo);
            }
            catch (Exception Ex)
            {
                Debug.WriteLine($"Error: {Ex.Message}");
                return await Task.FromResult(false);
            }
        }

        public async Task<Models.Todo> GetItemAsync(int id)
        {
            return await _todoApiService.GetItemAsync(id);
        }

        public async Task<List<Models.Todo>> SearchTodosAsync()
        {
            return await _todoApiService.SearchTodosAsync(SearchTerm);
        }
    }
}
