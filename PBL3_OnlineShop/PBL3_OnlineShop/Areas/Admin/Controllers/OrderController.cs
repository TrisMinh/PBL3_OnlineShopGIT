using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Services.Admin.Order;
using PBL3_OnlineShop.Validation;

namespace PBL3_OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RoleAuthorization("Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View(_orderService.GetAllOrders());
        }

        public IActionResult Comfirm(int id)
        {
            _orderService.ConfirmOrder(id);
            return RedirectToAction("Index");
        }

        public IActionResult Cancel(int id)
        {
            _orderService.CancelOrder(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Search(int? orderID, string customerName, int? status)
        {
            ViewBag.orderID = orderID;
            ViewBag.customerName = customerName;
            ViewBag.status = status;
            return View("Index", _orderService.SearchOrders(orderID,customerName,status));
        }
    }
}
