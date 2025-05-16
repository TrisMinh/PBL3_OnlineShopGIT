using PBL3_OnlineShop.Models.ViewModels;
using PBL3_OnlineShop.Models;

namespace PBL3_OnlineShop.Services.Account
{
    public interface IAccountService
    {
        // Register
        public bool IsUserNameExists(string username);
        public bool IsEmailExists(string email);
        public void CreateUser(RegisterView model);

        // Forgot Password
        public User FindByUsernameOrEmail(string input);
        public void ResetPassword(User user, string newPassword);

        // Login
        public User FindByUsername(string username);
        public bool VerifyPassword(User user, string password);

        // Profile
        public User GetUserById(int id);
        public bool UpdateProfile(int userId, string userName, string email, string phoneNumber, string gender, int day, int month, int year, string address);
        public bool ChangePassword(int userId, string currentPassword, string newPassword);

        // Avatar
        public Task<(bool success, string message, string avatarUrl)> UploadAvatar(int userId, IFormFile file);

        public void ApplySessionCartToUser(string tempCart, int? userId, ISession session);
    }

}
