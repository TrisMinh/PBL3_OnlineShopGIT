using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Services.Inventory;

namespace PBL3_OnlineShop.Services.Order
{
    public class OrderCusService : IOrderCusService
    {
        private readonly PBL3_Db_Context _context;
        private readonly IInventoryService _inventoryService;
        public OrderCusService(PBL3_Db_Context context, IInventoryService inventoryService)
        {
            _context = context;
            _inventoryService = inventoryService;
        }
        public List<Models.Order> GetAllOrdersByUserId(int? userId)
        {
            return _context.Orders
                .OrderByDescending(o => o.Id)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Include(o => o.User)
                .Where(o => o.UserId == userId)
                .ToList();
        }

        public Models.User GetUserById(int? userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public void CancelOrder(int id)
        {
            var order = _context.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.Id == id);

            List<int> productIds = new List<int>();
            foreach (var item in order.OrderDetails)
            {
                var product = _context.ProductsSize.FirstOrDefault(ps => ps.ProductId == item.ProductId && ps.Color == item.Color && ps.Size == item.Size);
                product.Quantity += item.Quantity;
                productIds.Add(item.ProductId);
            }
            order.Status = 0;
            _context.SaveChanges();
            _inventoryService.UpdateProductsStockQuantity(productIds);
            return;
        }

        public List<Models.Order> SearchOrder(int? status)
        {
            var query = from p in _context.Orders.OrderByDescending(p => p.Id).Include(p => p.OrderDetails).ThenInclude(od => od.Product).Include(p => p.User)
                        where (!status.HasValue || status == p.Status)
                        select p;
            return query.ToList();
        }
    }
}
