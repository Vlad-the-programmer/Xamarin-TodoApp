using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Todo.Helpers.RequestHelpers;
using Xamarin.Forms;
using LoginDto = Todo.ApiServices.UserLoginDto;
using RegisterDto = Todo.ApiServices.UserRegistrationDto;
using TodoClient = Todo.ApiServices.Client;
using TodoModel = Todo.ApiServices.Todo;
using UserModel = Todo.ApiServices.User;

namespace Todo.Services
{
    class UserDataStore : IUserDatStore
    {
        private readonly TodoClient _apiClient;

        private static ObservableCollection<UserModel> users;

        public UserDataStore()
        {
            _apiClient = DependencyService.Get<TodoClient>();
            users = (ObservableCollection<UserModel>)_apiClient.UsersAllAsync().GetAwaiter().GetResult();
        }


        public async Task<bool> Register(RegisterDto user)
            => await _apiClient.RegisterAsync(user).HandleRequest();

        public async Task<bool> Login(LoginDto user)
            => await _apiClient.LoginAsync(user).HandleRequest();

        public async Task<List<TodoModel>> GetUserTodos(int userId)
            => (List<TodoModel>)TodoService.AllTodos.Where(t => t.UserId == userId);

        public async Task<UserModel> GetUserBy(string username)
            => users.FirstOrDefault(u => u.Username == username);
    }
}
