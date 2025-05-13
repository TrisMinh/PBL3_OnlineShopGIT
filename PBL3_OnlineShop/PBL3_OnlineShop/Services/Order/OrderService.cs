using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Data;

namespace PBL3_OnlineShop.Services.Order
{
    public class OrderCusService : IOrderCusService
    {
        private readonly PBL3_Db_Context _context;
        public OrderCusService(PBL3_Db_Context context)
        {
            _context = context;
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
            UpdateProductsStockQuantity(productIds);
            return;
        }

        public List<Models.Order> SearchOrder(int? status)
        {
            var query = from p in _context.Orders.OrderByDescending(p => p.Id).Include(p => p.OrderDetails).ThenInclude(od => od.Product).Include(p => p.User)
                        where (!status.HasValue || status == p.Status)
                        select p;
            return query.ToList();
        }

        public void UpdateProductsStockQuantity(List<int> productIds)
        {
            foreach (var productId in productIds)
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
                if (product != null)
                {
                    // Tính tổng số lượng từ ProductsSize
                    var totalQuantity = _context.ProductsSize
                        .Where(ps => ps.ProductId == productId)
                        .Sum(ps => ps.Quantity);

                    // Cập nhật StockQuantity
                    product.StockQuantity = totalQuantity;
                    _context.Products.Update(product);
                }
            }
            _context.SaveChanges();
        }
    }
}
