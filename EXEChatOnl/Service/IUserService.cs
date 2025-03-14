using EXEChatOnl.DTOs;
using EXEChatOnl.Models;

namespace EXEChatOnl.Service
{
    public interface IUserService
    {
        RegistrationResponse RegisterUser(RegisterUserDTOs registerUserDTOs);

        LoginResponse LoginUser(LoginDTOs loginDTOs);
        
        ProfileUserRequest GetUserByUsername(string username);
        
        void UpdateUserByUsername(ProfileUserRequest userRequest);
        
        bool ChangePassword(string username, string oldPassword, string newPassword);
        
        IEnumerable<ProfileUserRequest> GetUsers();
    }
}
