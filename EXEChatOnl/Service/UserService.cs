using EXEChatOnl.DTOs;
using EXEChatOnl.Models;
using EXEChatOnl.Repository;
using Microsoft.CodeAnalysis.Scripting;

namespace EXEChatOnl.Service
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IJwtService _jwtService;
        public UserService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public LoginResponse LoginUser(LoginDTOs loginDTOs)
        {
            var getUser = _userRepository.GetUserByLogin(loginDTOs.Username);
            if (getUser != null)
            {
                bool checkPassword = BCrypt.Net.BCrypt.Verify(loginDTOs.Password, getUser.Password);
                if (checkPassword)
                {
                    string token = _jwtService.GenerateJWTToken(getUser);
                    bool isAdmin = getUser.Username == "admin";

                    return new LoginResponse(true, "Login successfully!", token, getUser.Username, isAdmin);
                }
            }
    
            return new LoginResponse(false, "Login failed!");
        }


        public RegistrationResponse RegisterUser(RegisterUserDTOs registerUserDTOs)
        {
            var getUser =  _userRepository.GetUserByLogin(registerUserDTOs.Username);
            if (getUser != null)
                return new RegistrationResponse(true, "Register failed!");
            _userRepository.AddUser(new User()
            {
                Username = registerUserDTOs.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(registerUserDTOs.Password),
            });
            return new RegistrationResponse(true, "Register successfully");
        }
    }
}
