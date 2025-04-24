using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Todo.Services
{
    public interface IDataStore<T>
    {
        string SearchTerm { get; set; }

        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(T item);
        Task<T> GetItemAsync(int id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }
}
