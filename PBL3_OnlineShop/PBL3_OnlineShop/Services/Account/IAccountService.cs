using PBL3_OnlineShop.Models.ViewModels;
using PBL3_OnlineShop.Models;

namespace PBL3_OnlineShop.Services.Account
{
    public interface IAccountService
    {
        // Register
        bool IsUserNameExists(string username);
        bool IsEmailExists(string email);
        void CreateUser(RegisterView model);

        // Forgot Password
        User FindByUsernameOrEmail(string input);
        void ResetPassword(User user, string newPassword);

        // Login
        User FindByUsername(string username);
        bool VerifyPassword(User user, string password);

        // Profile
        User GetUserById(int id);
    }

}
