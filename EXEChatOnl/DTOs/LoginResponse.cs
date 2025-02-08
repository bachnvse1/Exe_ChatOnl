using EXEChatOnl.Models;

namespace EXEChatOnl.DTOs
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; }

        public LoginResponse(bool success, string message, string token = null, string username = null, bool isAdmin = false)
        {
            Success = success;
            Message = message;
            Token = token;
            Username = username;
            IsAdmin = isAdmin;
        }
    }

}
