using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Models;

namespace Todo.Helpers
{
    internal class SearchFuncs
    {
        public static void Search(string searchTerm, ref string _searchTerm, IList<Models.Todo> AllTodos, IList<Models.Todo> Todos)
        {
            _searchTerm = searchTerm;
            IList<Models.Todo> foundTodos;
            if (!String.IsNullOrWhiteSpace(searchTerm))
                foundTodos = AllTodos.Where(todo => todo.Text.ToLower().Contains(searchTerm.ToLower())).ToList();
            else
                foundTodos = AllTodos;

            if (!foundTodos.SequenceEqual(Todos))
            {
                Todos.Clear();
                foreach (var todo in foundTodos)
                    Todos.Add(todo);
            }
        }
    }
}
