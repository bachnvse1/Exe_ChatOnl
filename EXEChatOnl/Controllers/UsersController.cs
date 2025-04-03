using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EXEChatOnl.Models;
using EXEChatOnl.DTOs;
using EXEChatOnl.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;

namespace EXEChatOnl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("_myAllowSpecificOrigins")] // Áp dụng CORS
    public class UsersController : ControllerBase
    {
        private readonly MyDbContext _context;
        private IUserService _userService;
        private IMapper _mapper;
        private MailUtils.MailUtils _mailUtils;

        public UsersController(MyDbContext context, IUserService userService, IMapper mapper)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
        }
        
        [HttpGet("AllUsers")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUserAll()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "admin")
            {
                return Forbid();
            }
            var user = _userService.GetUsers();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        
        // GET: api/Users/string
        [HttpGet("username/{username}")]
        [Authorize]
        public async Task<ActionResult<ProfileUserRequest>> GetUserProfile(string username)
        {
            var currentUser = User.Identity.Name;
            if (currentUser != username && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var user = _userService.GetUserByUsername(username);
            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> GetUser([FromBody] LoginDTOs user)
        {
            var result = _userService.LoginUser(user);
            if (!result.Success)
            {
                return Unauthorized(new { message = "Sai tên đăng nhập hoặc mật khẩu || Tài khoản đã bị ban" });
            }

            return Ok(result);
        }


        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update/{username}")]
        [Authorize]
        public async Task<IActionResult> PutUser(ProfileUserRequest user)
        {
         
            var currentUser = User.Identity.Name;
            if (currentUser != user.Username && !User.IsInRole("Admin"))
            {
                return Forbid();
            }
            _userService.UpdateUserByUsername(user);
            return NoContent();
        }
        
        [HttpPut("updatePassword/{username}")]
        [Authorize]
        public async Task<IActionResult> PutUserPassword(string username, [FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUser = User.Identity.Name;
            if (currentUser != username && !User.IsInRole("Admin"))
            {
                return Forbid();
            }
            bool result = _userService.ChangePassword(username, request.Password, request.NewPassword);
            return Ok(new { success = result });

        }


        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("register")]
        public async Task<ActionResult<User>> PostUser(RegisterUserDTOs user)
        {
            var result = _userService.RegisterUser(user);
            return Ok(result);
        }

        // DELETE: api/Users/5
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "admin")
            {
                return Forbid();
            }
            var user = _context.Users.FirstOrDefault(x=>x.Username == username);
            var userRoles = _context.UserRoles.Where(x=>x.UserId == user.Id).First();
            if (user == null || userRoles.RoleName == "admin")
            {
                return NotFound();
            }

            user.IsDeleted = !user.IsDeleted;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        
        [HttpPut("updateStatus/{username}")]
        public async Task<IActionResult> UpdateUserStatus(string username, [FromBody] UpdateUserRequest jsonData)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "admin")
            {
                return Forbid();
            }
            var user = _context.Users.FirstOrDefault(x=>x.Username == username);
            if (user == null)
            {
                return NotFound("User không tồn tại.");
            }
            

            if (jsonData == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            if (!string.IsNullOrEmpty(jsonData.Role))
            {
                var userRoleOld = _context.UserRoles.Where(x => x.UserId == user.Id).First();
                userRoleOld.RoleName = jsonData.Role;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }
        
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request?.Email))
            {
                return BadRequest("Email is required.");
            }
            var user = _context.Users.FirstOrDefault(x => x.Email.ToLower() == request.Email.ToLower());
            if (user == null)
                return StatusCode(500, new { flag = true, message = "Email không tồn tại trong dữ liệu của bạn" });
            
            string newPassword = GenerateRandomPassword();

        
            string emailBody = $@"
    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; background-color: #f9f9f9;'>
        <h2 style='color: #333; text-align: center;'>🔑 Password Reset Request</h2>
        <p style='font-size: 16px; color: #555;'>Hello,</p>
        <p style='font-size: 16px; color: #555;'>
            We have generated a new password for your account. Please use the password below to log in and consider changing it immediately.
        </p>
        <div style='text-align: center; padding: 15px 0;'>
            <span style='font-size: 18px; font-weight: bold; color: #fff; background-color: #007bff; padding: 10px 20px; border-radius: 5px; display: inline-block;'>
                {newPassword}
            </span>
        </div>
        <p style='font-size: 16px; color: #555;'>
            If you did not request a password reset, please ignore this email or contact support.
        </p>
        <p style='font-size: 14px; color: #777; text-align: center; margin-top: 20px;'>
            &copy; 2025 Your Company. All rights reserved.
        </p>
    </div>";

            string result = await MailUtils.MailUtils.sendGMail("Bachnvhe172297@fpt.edu.vn", request.Email, "Password Reset", emailBody, "Bachnvhe172297@fpt.edu.vn", "iggr juzv mnaw jqbj");

            if (result == "Sucess")
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return Ok(new { flag = false, message = "New password has been sent to your email." });
            }
            else
            {
                return StatusCode(500, new { flag = true, message = "Failed to send email." });
            }
        }

        private static string GenerateRandomPassword(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder password = new StringBuilder();
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[length];
                rng.GetBytes(data);
                foreach (byte b in data)
                {
                    password.Append(chars[b % chars.Length]);
                }
            }
            return password.ToString();
        }
        
        public class UpdateUserRequest
        {
            public string Role { get; set; }
        }
        public class ForgotPasswordRequest
        {
            public string Email { get; set; }
        }

    }
}
