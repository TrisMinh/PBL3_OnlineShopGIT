namespace PBL3_OnlineShop.Services.Order
{
    public interface IOrderCusService
    {
        public List<Models.Order> GetAllOrdersByUserId(int userId);
        public Models.User GetUserById(int userId);
        public void CancelOrder(int id);
        public List<Models.Order> SearchOrder(int? status);
    }
}
