using Microsoft.AspNetCore.Identity;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Models.ViewModels;
using PBL3_OnlineShop.Models;

namespace PBL3_OnlineShop.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly PBL3_Db_Context _context;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public AccountService(PBL3_Db_Context context)
        {
            _context = context;
        }

        public bool IsUserNameExists(string username)
        {
            return _context.Users.Any(u => u.UserName == username);
        }
            

        public bool IsEmailExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
            

        public void CreateUser(RegisterView model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
                Gender = "Man",
                UrlAvatar = "/avatar/def.jpg",
                Role = "Customer",
                CreatedAt = DateTime.Now,
                Status = 1
            };
            user.Password = _passwordHasher.HashPassword(user, model.Password);
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User FindByUsernameOrEmail(string input) =>
            _context.Users.FirstOrDefault(u => u.UserName == input || u.Email == input);

        public void ResetPassword(User user, string newPassword)
        {
            user.Password = _passwordHasher.HashPassword(user, newPassword);
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public User FindByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == username);
        }
            

        public bool VerifyPassword(User user, string password)
        {
            return _passwordHasher.VerifyHashedPassword(user, user.Password, password) != PasswordVerificationResult.Failed;
        }

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }
    }

}
