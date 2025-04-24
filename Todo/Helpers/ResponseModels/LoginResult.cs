using System;
using Todo.Models;

namespace Todo.Helpers.ResponseModels
{
    public class LoginResult
    {
        public string Token { get; set; }
        public User User { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
