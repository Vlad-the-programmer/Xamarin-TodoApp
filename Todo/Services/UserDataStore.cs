using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Todo.Helpers.RequestHelpers;
using Todo.Helpers.ResponseModels;
using Todo.Models;
using Xamarin.Forms;
using TodoClient = Todo.Todo.ApiServices.Client;
using TodoModel = Todo.Todo.ApiServices.Todo;
using UserModel = Todo.Todo.ApiServices.User;

namespace Todo.Services
{
    class UserDataStore : IUserDatStore
    {
        private readonly TodoClient _apiClient;

        private static ObservableCollection<User> users;


        public UserDataStore()
        {
            //users = (ObservableCollection<User>)GetItemsAsync(true).GetAwaiter().GetResult();
            _apiClient = DependencyService.Get<TodoClient>();
        }


        public async Task<bool> Register(UserModel user)
            => await _apiClient.RegisterAsync(user).HandleRequest();

        public async Task<bool> Login(LoginUserModel user)
            => await _apiClient.LoginAsync(user).HandleRequest();

        public async Task<List<TodoModel>> GetUserTodos(int userId)
            => (List<TodoModel>)TodoService.AllTodos.Where(t => t.UserId == userId);

        public async Task<UserModel> GetUserBy(string username)
            => new UserModel();
    }
}
