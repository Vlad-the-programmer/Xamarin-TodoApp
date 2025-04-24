using System.Collections.Generic;
using System.Threading.Tasks;

namespace Todo.Services
{
    public interface IUserDatStore
    {
        Task<bool> Register(Models.User user);
        Task<bool> Login(string login, string password);
        Task<List<Models.Todo>> GetUserTodos(int userId);
        Task<Models.User> GetUserBy(string username);
    }
}
