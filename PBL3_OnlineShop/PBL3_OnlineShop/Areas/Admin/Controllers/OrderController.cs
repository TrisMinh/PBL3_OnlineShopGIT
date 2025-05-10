using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Repository;

namespace PBL3_OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly PBL3_Db_Context _context;
        public OrderController(PBL3_Db_Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Product).Include(o => o.User).ToList());
        }
        public IActionResult Comfirm(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            order.Status = 2;
            _context.Orders.Update(order);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Cancel(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            order.Status = 0;
            _context.Orders.Update(order);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
