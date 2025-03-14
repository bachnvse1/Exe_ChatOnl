using EXEChatOnl.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EXEChatOnl.DTOs;

namespace EXEChatOnl.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<ProfileUserRequest> GetAllUsers()
        {   
            var users = _context.Users
                .Include(u=>u.UserRoles)
                .Include(c=>c.Customer).ToList();
            var mappedUsers = _mapper.Map<IEnumerable<ProfileUserRequest>>(users);
            return mappedUsers;
        }

        public User? GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public User? GetUserByLogin(string username)
        {
            return _context.Users.Include(u=>u.UserRoles).FirstOrDefault(u => u.Username == username);
        }
    }
}
