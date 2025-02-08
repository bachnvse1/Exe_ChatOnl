using EXEChatOnl.Models;

namespace EXEChatOnl.Service
{
    public interface IJwtService
    {
        string GenerateJWTToken(User user);
    }
}
