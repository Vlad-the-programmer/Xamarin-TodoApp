using System;
using System.Collections.Generic;

namespace Todo.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; } = null;

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public bool IsLoggedIn { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime? RefreshTokenExpiry { get; set; }
        public IList<Models.Todo> Todos { get; set; } = new List<Models.Todo>();
    }
}
