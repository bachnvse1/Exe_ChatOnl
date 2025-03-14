 using EXEChatOnl.Models;
using System.Collections.Generic;
using EXEChatOnl.DTOs;

namespace EXEChatOnl.Repository
{
    public interface IUserRepository
    {
        IEnumerable<ProfileUserRequest> GetAllUsers();
        User? GetUserById(int id);
        User? GetUserByLogin(string username);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}