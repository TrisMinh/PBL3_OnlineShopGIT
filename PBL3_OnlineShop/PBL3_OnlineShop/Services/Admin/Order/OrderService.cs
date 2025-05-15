using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Services.Inventory;

namespace PBL3_OnlineShop.Services.Admin.Order
{
    public class OrderService : IOrderService
    {
        private readonly PBL3_Db_Context _context;
        private readonly IInventoryService _inventoryService;
        public OrderService(PBL3_Db_Context context, IInventoryService inventoryService)
        {
            _context = context;
            _inventoryService = inventoryService;
        }
        public List<Models.Order> GetAllOrders()
        {
            return _context.Orders.OrderByDescending(o => o.Id).Include(o => o.OrderDetails).ThenInclude(od => od.Product).Include(o => o.User).ToList();
        }

        public void ConfirmOrder(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            order.Status = 2;
            _context.SaveChanges();
            return;
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

            if (order.CouponUsed != "No")
            {
                var coupon = _context.Coupons.FirstOrDefault(c => c.Name == order.CouponUsed);
                var couponusage = _context.CouponUsages.FirstOrDefault(cu => cu.CouponId == coupon.Id && cu.UserId == order.UserId);
                if (couponusage != null)
                {
                    _context.CouponUsages.Remove(couponusage);
                    coupon.Quantity += 1;
                    _context.Coupons.Update(coupon);
                    _context.SaveChanges();
                }
            }

            order.Status = 0;
            _context.SaveChanges();
            _inventoryService.UpdateProductsStockQuantity(productIds);
            return;
        }
        
        public List<Models.Order> SearchOrders(int? orderID, string customerName, int? status)
        {
            var query = from p in _context.Orders.Include(p => p.OrderDetails).ThenInclude(od => od.Product).Include(p => p.User)
                        where (!orderID.HasValue || orderID == p.Id) &&
                        (string.IsNullOrEmpty(customerName) || p.User.UserName.Contains(customerName)) &&
                        (!status.HasValue || status == p.Status)
                        select p;
            var orders = query.ToList();        
            return orders;
        }
    }
}
