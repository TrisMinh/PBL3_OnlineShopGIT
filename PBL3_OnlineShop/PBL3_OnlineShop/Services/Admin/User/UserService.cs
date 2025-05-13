
using Microsoft.AspNetCore.Identity;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Models;

namespace PBL3_OnlineShop.Services.Admin.User
{
    public class UserService : IUserService
    {
        private readonly PBL3_Db_Context _context;
        private readonly PasswordHasher<Models.User> _passwordHasher = new();
        public UserService(PBL3_Db_Context context)
        {
            _context = context;
        }

        public List<Models.User> GetAllUsers()
        {
            return _context.Users.OrderByDescending(p => p.Id).ToList();
        }

        public Models.User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(p => p.Id == id);
        }

        public bool CreateUser(Models.User user)
        {
            if (_context.Users.Any(p => p.UserName == user.UserName))
            {
                return false;
            }
            user.Password = _passwordHasher.HashPassword(user, user.Password);
            _context.Users.Add(user);
            _context.SaveChanges();
            return true;
        }

        public bool UpdateUser(int id, Models.User user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null) return false;

            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.DateOfBirth = user.DateOfBirth;
            existingUser.Role = user.Role ?? "Customer";
            existingUser.Address = user.Address;
            existingUser.Status = user.Status;

            _context.Users.Update(existingUser);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(p => p.Id == id);
            if (user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
    }
}
