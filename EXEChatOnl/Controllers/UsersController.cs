using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
                return Unauthorized(new { message = "Sai tên đăng nhập hoặc mật khẩu" });
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
