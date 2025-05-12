using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Data;

namespace PBL3_OnlineShop.Controllers
{
    public class OrderController : Controller
    {
        public readonly PBL3_Db_Context _context;
        public OrderController(PBL3_Db_Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("_UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            
            // Tìm người dùng từ database để lấy thông tin đầy đủ
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            ViewBag.User = user;
            
            var orders = _context.Orders
                .OrderByDescending(o => o.Id)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Include(o => o.User)
                .Where(o => o.UserId == userId)
                .ToList();
            return View(orders);
        }
        public IActionResult Cancel(int id)
        {
            var order = _context.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.Id == id);

            foreach (var item in order.OrderDetails)
            {
                var product = _context.ProductsSize.FirstOrDefault(ps => ps.ProductId == item.ProductId && ps.Color == item.Color && ps.Size == item.Size);
                product.Quantity += item.Quantity;
                _context.ProductsSize.Update(product);
            }

            order.Status = 0;
            _context.Orders.Update(order);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Search(int? status)
        {
            var query = from p in _context.Orders.OrderByDescending(p => p.Id).Include(p => p.OrderDetails).ThenInclude(od => od.Product).Include(p => p.User)
                        where (!status.HasValue || status == p.Status)
                        select p;
            var orders = query.ToList();

            ViewBag.status = status;

            return View("Index", orders);
        }
    }
}
