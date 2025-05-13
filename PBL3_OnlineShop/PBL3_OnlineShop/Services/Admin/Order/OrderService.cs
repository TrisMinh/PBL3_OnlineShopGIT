using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Data;

namespace PBL3_OnlineShop.Services.Admin.Order
{
    public class OrderService : IOrderService
    {
        private readonly PBL3_Db_Context _context;
        public OrderService(PBL3_Db_Context context)
        {
            _context = context;
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

            foreach (var item in order.OrderDetails)
            {
                var product = _context.ProductsSize.FirstOrDefault(ps => ps.ProductId == item.ProductId && ps.Color == item.Color && ps.Size == item.Size);
                product.Quantity += item.Quantity;
            }
            order.Status = 0;
            _context.SaveChanges();
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
