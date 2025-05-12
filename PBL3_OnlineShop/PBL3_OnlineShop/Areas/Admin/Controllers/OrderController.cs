using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Repository;
using PBL3_OnlineShop.Validation;

namespace PBL3_OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RoleAuthorization("Admin")]
    public class OrderController : Controller
    {
        private readonly PBL3_Db_Context _context;
        public OrderController(PBL3_Db_Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Orders.OrderByDescending(o => o.Id).Include(o => o.OrderDetails).ThenInclude(od => od.Product).Include(o => o.User).ToList());
        }
        public IActionResult Comfirm(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            order.Status = 2;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Cancel(int id)
        {
            var order = _context.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.Id == id);

            foreach(var item in order.OrderDetails)
            {
                var product = _context.ProductsSize.FirstOrDefault(ps => ps.ProductId == item.ProductId && ps.Color == item.Color && ps.Size == item.Size);
                product.Quantity += item.Quantity;
            }

            order.Status = 0;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Search(int? orderID, string customerName, int? status)
        {
            var query = from p in _context.Orders.Include(p => p.OrderDetails).ThenInclude(od => od.Product).Include(p => p.User)
                        where (!orderID.HasValue || orderID == p.Id) &&
                        (string.IsNullOrEmpty(customerName) || p.User.Name.Contains(customerName))&&
                        (!status.HasValue || status == p.Status)
                        select p;
            var orders = query.ToList();

            ViewBag.orderID = orderID;
            ViewBag.customerName = customerName;
            ViewBag.status = status;

            return View("Index", orders);
        }
    }
}
