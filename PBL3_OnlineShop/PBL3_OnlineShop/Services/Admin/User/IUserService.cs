namespace PBL3_OnlineShop.Services.Admin.User
{
    public interface IUserService
    {
        public List<Models.User> GetAllUsers();
        public Models.User GetUserById(int id);
        public bool CreateUser(Models.User user);
        public bool UpdateUser(int id, Models.User user);
        public bool DeleteUser(int id);
    }
}
