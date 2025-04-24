using Todo.Models;
using Todo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Helpers
{
    class UserAuth
    {
        public IDataStore<User> _usersDataStore;

        public UserAuth(IDataStore<User> userDataStore)
        {
            _usersDataStore = userDataStore;
        }

        public async Task<User> GetUserAsync(string username)
        {
            try
            {
                return (await _usersDataStore.GetItemsAsync())
                                        .ToList()
                                        .FirstOrDefault(u => u.Username == username);

            } catch(NullReferenceException e)
            {
                return null;
            }
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return ( await GetUserAsync(username) ) != null;
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            User user = await GetUserAsync(username);
            if (user == null) return false;
            return true;
        }

    }
}
