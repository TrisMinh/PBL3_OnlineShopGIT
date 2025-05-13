using PBL3_OnlineShop.Models;
namespace PBL3_OnlineShop.Services.Admin.Order
{
    public interface IOrderService
    {
        public List<Models.Order> GetAllOrders();
        public void ConfirmOrder(int id);
        public void CancelOrder(int id);
        public List<Models.Order> SearchOrders(int? orderID, string customerName, int? status);
        public void UpdateProductsStockQuantity(List<int> productIds);
    }
}
