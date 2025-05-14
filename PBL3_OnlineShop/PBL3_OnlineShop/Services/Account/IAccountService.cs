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

        // Profile
        public User GetUserById(int id);

        public void ApplySessionCartToUser(string tempCart, int? userId, ISession session);
    }

}
