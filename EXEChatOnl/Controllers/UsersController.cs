using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EXEChatOnl.Models;
using EXEChatOnl.DTOs;
using EXEChatOnl.Service;
using Microsoft.AspNetCore.Cors;

namespace EXEChatOnl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("_myAllowSpecificOrigins")] // Áp dụng CORS
    public class UsersController : ControllerBase
    {
        private readonly DBContext _context;
        private IUserService _userService;

        public UsersController(DBContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: api/Users
/*        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.ToListAsync();
        }*/

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
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
