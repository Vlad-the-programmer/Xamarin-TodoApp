using System.Collections.Generic;
using System.Threading.Tasks;

namespace Todo.Services
{
    public interface ITodoService : IDataStore<Models.Todo>
    {
        Task<List<Models.Todo>> SearchTodosAsync();
    }
}
