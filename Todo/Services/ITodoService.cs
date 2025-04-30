using System.Collections.Generic;
using System.Threading.Tasks;

using TodoModel = Todo.ApiServices.Todo;

namespace Todo.Services
{
    public interface ITodoService : IDataStore<TodoModel>
    {
        Task<List<TodoModel>> SearchTodosAsync();
    }
}
