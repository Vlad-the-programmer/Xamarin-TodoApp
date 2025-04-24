using Newtonsoft.Json;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Models;

namespace Todo.Helpers
{
    internal class DbFileHandler
    {
        public static async Task<IFile> OpenFile(string folderName, string fileName)
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder folder = await rootFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
            IFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

            return file;
        }
        public static async Task SaveTodos(IList<Models.Todo> AllTodos)
        {
            var file = await OpenFile("Todos", "todos.qwerty");

            var json = JsonConvert.SerializeObject(AllTodos);
            await file.WriteAllTextAsync(json);
        }

        public static async Task SaveUsers(List<User> users)
        {
            var file = await OpenFile("Users", "users.qwerty");

            var json = JsonConvert.SerializeObject(users);
            await file.WriteAllTextAsync(json);
        }
    }
}
