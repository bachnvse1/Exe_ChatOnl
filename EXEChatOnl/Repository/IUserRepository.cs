using EXEChatOnl.Models;
using System.Collections.Generic;

namespace EXEChatOnl.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User? GetUserById(int id);
        User? GetUserByLogin(string username);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}