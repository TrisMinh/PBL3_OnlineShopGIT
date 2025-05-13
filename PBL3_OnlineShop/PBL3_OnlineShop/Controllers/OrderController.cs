using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Services.Order;

namespace PBL3_OnlineShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderCusService _orderCusService;
        public OrderController(IOrderCusService orderCusService)
        {
            _orderCusService = orderCusService;
        }
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("_UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _orderCusService.GetUserById(userId);
            ViewBag.User = user;

            var orders = _orderCusService.GetAllOrdersByUserId(userId.Value);
            return View(orders);
        }
        public IActionResult Cancel(int id)
        {
            _orderCusService.CancelOrder(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Search(int? status)
        {
            var orders = _orderCusService.SearchOrder(status);

            ViewBag.status = status;

            return View("Index", orders);
        }
    }
}
