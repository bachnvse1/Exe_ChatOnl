using EXEChatOnl.DTOs;
using EXEChatOnl.Models;

namespace EXEChatOnl.Service
{
    public interface IUserService
    {
        RegistrationResponse RegisterUser(RegisterUserDTOs registerUserDTOs);

        LoginResponse LoginUser(LoginDTOs loginDTOs);
    }
}
