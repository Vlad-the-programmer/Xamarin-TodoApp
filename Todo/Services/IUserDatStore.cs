using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Helpers.ResponseModels;
using TodoModel = Todo.Todo.ApiServices.Todo;
using UserModel = Todo.Todo.ApiServices.User;

namespace Todo.Services
{
    public interface IUserDatStore
    {
        Task<bool> Register(UserModel user);
        Task<bool> Login(LoginUserModel user);
        Task<List<TodoModel>> GetUserTodos(int userId);
        Task<UserModel> GetUserBy(string username);
    }
}
