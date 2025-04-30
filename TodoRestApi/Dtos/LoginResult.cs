using TodoRestApi.Models;


namespace TodoRestApi.Dtos
{
    public class LoginResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public User User { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
