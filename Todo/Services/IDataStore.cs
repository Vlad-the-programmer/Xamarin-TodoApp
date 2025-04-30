using System.Collections.Generic;
using System.Threading.Tasks;

namespace Todo.Services
{
    public interface IDataStore<T>
    {
        string SearchTerm { get; set; }

        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(int id, T item);
        Task<bool> DeleteItemAsync(int id);
        Task<T> GetItemAsync(int id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }
}
