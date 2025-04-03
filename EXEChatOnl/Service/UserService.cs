using AutoMapper;
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
        private MyDbContext _context;
        private IMapper _mapper;
        public UserService(IUserRepository userRepository, IJwtService jwtService, MyDbContext context, IMapper mapper)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _context = context;
            _mapper = mapper;
        }

        public LoginResponse LoginUser(LoginDTOs loginDTOs)
        {
            var getUser = _userRepository.GetUserByLogin(loginDTOs.Username);
            var userRoles = _context.UserRoles.Where(u => u.UserId == getUser.Id).First();
            if (getUser != null && getUser.IsDeleted == false)
            {
                bool checkPassword = BCrypt.Net.BCrypt.Verify(loginDTOs.Password, getUser.Password);
                if (checkPassword)
                {
                    string token = _jwtService.GenerateJWTToken(getUser);
                    bool isAdmin = userRoles.RoleName == "admin";

                    return new LoginResponse(true, "Login successfully!", token, getUser.Username, isAdmin);
                }
            }
    
            return new LoginResponse(false, "Login failed!");
        }

        public ProfileUserRequest GetUserByUsername(string username)
        {
            var getUser = _userRepository.GetUserByLogin(username);
            var getUserDtos = _mapper.Map<ProfileUserRequest>(getUser);
            var UserRole = _context.UserRoles.Where(x => x.UserId == getUser.Id);
            var Customer = _context.Customers.Where(x => x.UserId == getUser.Id);
            getUserDtos.RoleName = UserRole.Select(x=>x.RoleName).ToList();
            getUserDtos.FullName = Customer.Select(x=>x.FullName).FirstOrDefault();
            getUserDtos.Email = getUser.Email;
            getUserDtos.Phone = Customer.Select(x=>x.Phone).FirstOrDefault();
            getUserDtos.Address = Customer.Select(x=>x.Address).FirstOrDefault();
            if (getUser != null && getUserDtos != null)
            {
                return getUserDtos;
            } 
            return null;
        }

        public void UpdateUserByUsername(ProfileUserRequest userRequest)
        {
            var getUser = _userRepository.GetUserByLogin(userRequest.Username);
            var getUserDtos = _mapper.Map<ProfileUserRequest>(getUser);
            try
            {
                if (getUser != null && getUserDtos != null)
                {
                    getUser.Email = userRequest.Email;
                    _context.Users.Update(getUser);
                    
                    var customer = _context.Customers.Where(x => x.UserId == getUser.Id).FirstOrDefault();
                    if (customer != null)
                    {
                        customer.FullName = userRequest.FullName;
                        customer.Phone = userRequest.Phone;
                        customer.Address = userRequest.Address;
                        _context.Customers.Update(customer);

                    } else throw new Exception("User not found");
                    
                    _context.SaveChanges();
                } else throw new Exception("User not found");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var getUser = _userRepository.GetUserByLogin(username);
            try
            {
                if (getUser != null &&  BCrypt.Net.BCrypt.Verify(oldPassword, getUser.Password))
                {
                    getUser.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    _userRepository.UpdateUser(getUser);
                    return true;
                } else throw new Exception("Passwords do not match!");
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        public IEnumerable<ProfileUserRequest> GetUsers()
        {
            var getAllUsers = _userRepository.GetAllUsers();
            return getAllUsers;
        }


        public RegistrationResponse RegisterUser(RegisterUserDTOs registerUserDTOs)
        {
            var getUser =  _userRepository.GetUserByLogin(registerUserDTOs.Username);
            if (getUser != null)
                return new RegistrationResponse(true, "Username is exist!");
            _userRepository.AddUser(new User()
            {
                Username = registerUserDTOs.Username,
                Email = registerUserDTOs.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerUserDTOs.Password),
            });
            
            _context.UserRoles.Add(new UserRole()
            {
                UserId = _userRepository.GetUserByLogin(registerUserDTOs.Username).Id,
                RoleName = "employee"
            });
            
            _context.SaveChanges();
            
            _context.Customers.Add(new Customer()
            {
                UserId = _userRepository.GetUserByLogin(registerUserDTOs.Username).Id,
                FullName = registerUserDTOs.Username,
                Phone = "12345678",
                Address = "Exxample address",
            });
            _context.SaveChanges();
            return new RegistrationResponse(false, "Đăng kí thành công!");
        }
    }
}
