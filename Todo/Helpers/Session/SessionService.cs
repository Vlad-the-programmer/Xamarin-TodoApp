using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Models;

namespace Todo.Helpers.Session
{
    public class SessionService
    {
        private static SessionService _instance;
        public static SessionService Instance => _instance ?? (_instance = new SessionService());

        public User CurrentUser { get; set; } = null;
        public IList<Models.Todo> UserTodos { get; set; } = new List<Models.Todo>();

        public void Logout()
        {
            CurrentUser = null;
            UserTodos.Clear(); // Clear the list instead of reassigning
        }
    }


}
