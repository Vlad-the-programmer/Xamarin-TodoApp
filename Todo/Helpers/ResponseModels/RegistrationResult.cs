using Todo.Models;

namespace Todo.Helpers.ResponseModels
{
    public class RegistrationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public User User { get; set; }
    }
}
