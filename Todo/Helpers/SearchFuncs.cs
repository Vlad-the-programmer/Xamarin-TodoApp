using System;
using System.Collections.Generic;
using System.Linq;

using TodoModel = Todo.Todo.ApiServices.Todo;

namespace Todo.Helpers
{
    internal class SearchFuncs
    {
        public static void Search(string searchTerm, ref string _searchTerm, IList<TodoModel> AllTodos, IList<TodoModel> Todos)
        {
            _searchTerm = searchTerm;
            IList<TodoModel> foundTodos;
            if (!String.IsNullOrWhiteSpace(searchTerm))
                foundTodos = AllTodos.Where(todo => todo.Content.ToLower().Contains(searchTerm.ToLower())).ToList();
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
