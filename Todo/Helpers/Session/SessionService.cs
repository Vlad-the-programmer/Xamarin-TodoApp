using System.Collections.Generic;
using TodoModel = Todo.ApiServices.Todo;
using UserModel = Todo.ApiServices.User;

namespace Todo.Helpers.Session
{
    public class SessionService
    {
        private static SessionService _instance;
        public static SessionService Instance => _instance ?? (_instance = new SessionService());

        public UserModel CurrentUser { get; set; } = null;
        public bool IsCurrentUserLoggedIn { get; set; } = false;
        public IList<TodoModel> UserTodos { get; set; } = new List<TodoModel>();

        public void Logout()
        {
            CurrentUser = null;
            IsCurrentUserLoggedIn = false;
            UserTodos.Clear(); // Clear the list instead of reassigning
        }
    }


}
