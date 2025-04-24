using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Services;

namespace Todo.ApiServices
{
    public interface ITodoApiService : IDataStore<Models.Todo>
    {
        Task<List<Models.Todo>> SearchTodosAsync(string searchTerm);
    }
}
