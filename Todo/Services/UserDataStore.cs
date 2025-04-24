using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Todo.ApiServices;
using Todo.Helpers.Exception;
using Todo.Models;

namespace Todo.Services
{
    class UserDataStore : IUserDatStore
    {
        private readonly IUsersApiService _usersApiService;

        private static ObservableCollection<User> users;


        public UserDataStore()
        {
            //users = (ObservableCollection<User>)GetItemsAsync(true).GetAwaiter().GetResult();
            _usersApiService = new UsersApiService();
        }



        public async Task<bool> Register(User user)
        {
            try
            {
                await _usersApiService.RegisterAsync(user);
                return true;
            }
            catch (ApiException Ex)
            {
                Debug.WriteLine($"Error: {Ex.Message} {Ex.InnerException.Message}");
                return false;
            }
        }

        public async Task<bool> Login(string login, string password)
        {
            try
            {
                await _usersApiService.LoginAsync(login, password);
                return true;
            }
            catch (ApiException Ex)
            {
                Debug.WriteLine($"Error: {Ex.Message}");
                return false;
            }
        }

        public async Task<List<Models.Todo>> GetUserTodos(int userId)
        {
            try
            {
                return await _usersApiService.GetUserTodosAsync(userId);
            }
            catch (ApiException Ex)
            {
                Debug.WriteLine($"Error: {Ex.Message}");
                return await Task.FromResult(new List<Models.Todo>());
            }
        }

        public async Task<User> GetUserBy(string username)
        {
            try
            {
                return await _usersApiService.GetUserAsync(username);
            }
            catch (ApiException Ex)
            {
                Debug.WriteLine($"Error: {Ex.Message}");
                return await Task.FromResult(new Models.User());
            }
        }
    }
}
