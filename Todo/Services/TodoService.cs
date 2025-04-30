using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Todo.Helpers.RequestHelpers;
using Xamarin.Forms;

using TodoClient = Todo.ApiServices.Client;
using TodoModel = Todo.ApiServices.Todo;


[assembly: Dependency(typeof(Todo.Services.TodoService))]
namespace Todo.Services
{
    public class TodoService : ITodoService
    {
        public static ObservableCollection<TodoModel> Todos = new ObservableCollection<TodoModel>();
        public static List<TodoModel> AllTodos = new List<TodoModel>();
        private static string _searchTerm;
        private readonly TodoClient _apiClient;

        public string SearchTerm { get { return _searchTerm; } set => _searchTerm = value; }



        public TodoService()
        {
            _apiClient = DependencyService.Get<TodoClient>();
            AllTodos = (List<TodoModel>)_apiClient.TodosAllAsync().GetAwaiter().GetResult();
        }

        public async Task<IEnumerable<TodoModel>> GetItemsAsync(bool forceRefresh = false)
            => AllTodos;


        public async Task<bool> AddItemAsync(TodoModel todo)
            => await _apiClient.TodosPOSTAsync(todo).HandleRequest();


        public async Task<bool> DeleteItemAsync(int id)
            => await _apiClient.TodosDELETEAsync(id).HandleRequest();

        public async Task<bool> UpdateItemAsync(int id, TodoModel todo)
            => await _apiClient.TodosPUTAsync(id, todo).HandleRequest();

        public async Task<TodoModel> GetItemAsync(int id)
        {
            var found = await _apiClient.TodosGETAsync(id).HandleRequest();
            if (!found)
                return null;

            return await _apiClient.TodosGETAsync(id);
        }

        public async Task<List<TodoModel>> SearchTodosAsync()
        {
            var found = await _apiClient.SearchAsync(_searchTerm).HandleRequest();
            if (!found)
                return null;

            return (List<TodoModel>)await _apiClient.SearchAsync(_searchTerm);
        }
    }
}
