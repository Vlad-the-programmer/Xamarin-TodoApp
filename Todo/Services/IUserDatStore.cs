using System.Collections.Generic;
using System.Threading.Tasks;
using LoginDto = Todo.ApiServices.UserLoginDto;
using RegisterDto = Todo.ApiServices.UserRegistrationDto;
using TodoModel = Todo.ApiServices.Todo;
using UserModel = Todo.ApiServices.User;

namespace Todo.Services
{
    public interface IUserDatStore
    {
        Task<bool> Register(RegisterDto user);
        Task<bool> Login(LoginDto user);
        Task<List<TodoModel>> GetUserTodos(int userId);
        Task<UserModel> GetUserBy(string username);
    }
}
