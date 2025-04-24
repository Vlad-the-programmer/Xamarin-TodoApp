using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Todo.Helpers.ResponseModels;

namespace Todo.ApiServices
{
    public interface IUsersApiService
    {
        //Task Register(Models.User user);
        //Task Login(string login, string password);
        //Task<List<Models.Todo>> GetUserTodos(int userId);
        //Task<Models.User> GetUserBy(string username);

        Task<Models.User> GetUserAsync(string username, CancellationToken cancellationToken = default);
        Task<List<Models.Todo>> GetUserTodosAsync(int userId, CancellationToken cancellationToken = default);
        Task<LoginResult> LoginAsync(string username, string password, CancellationToken cancellationToken = default);
        Task<RegistrationResult> RegisterAsync(Models.User user, CancellationToken cancellationToken = default);
    }
}
