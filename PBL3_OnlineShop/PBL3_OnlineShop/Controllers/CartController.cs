using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Models.ViewModels;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.SessionHelper;
using System.Drawing;
using PBL3_OnlineShop.Services.Cart;

namespace PBL3_OnlineShop.Controllers
{
    public class CartController : Controller
    {
        private readonly PBL3_Db_Context _context;
        private readonly ICartService _cartService;
        public CartController(PBL3_Db_Context context, ICartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }
        // GET: CartController
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("_UserId");
            var cartView = _cartService.GetCartView(userId, HttpContext.Session);
            return View(cartView);
        }

        [HttpPost]
        public IActionResult Checkout()
        {
            var userId = HttpContext.Session.GetInt32("_UserId");

            if (userId == null)
            {
                TempData["Error"] = "Please login to checkout.";
                return RedirectToAction("Login", "Account");
            }

            if (_cartService.CheckCartItem(userId) != "OK")
            {
                TempData["Error"] = _cartService.CheckCartItem(userId);
                return RedirectToAction("Index");
            }

            var user = _cartService.GetUserById(userId);
            if (user.UserName == null || user.PhoneNumber == null || user.Address == null)
            {
                TempData["Error"] = "Please enter complete contact information";
                return RedirectToAction("Profile", "Account");
            }

            return RedirectToAction("Index", "Checkout");
        }
        public IActionResult Add(int id, string size, string color)
        {
            // Kiểm tra
            var productSize = _cartService.GetProductSizeByIdSizeColor(id, size, color);
            if (productSize == null)
            {
                TempData["Error"] = "Product not found or size/color not available.";
                return Redirect(Request.Headers["Referer"].ToString()); // trả về trang hiện tại
            }

            var userId = HttpContext.Session.GetInt32("_UserId");
            Product product = _cartService.GetProductByID(id);
            if (product == null) return NotFound();

            _cartService.AddToCart(userId, id, size, color, HttpContext.Session, product);

            TempData["Success"] = "Added product to Cart";
            return Redirect(Request.Headers["Referer"].ToString()); // trả về trang hiện tại
        }

        [HttpPost]
        public IActionResult Increase(int id, string size, string color)
        {
            var userId = HttpContext.Session.GetInt32("_UserId"); // Kiểm tra nếu đã đăng nhập
            _cartService.IncreaseQuantity(userId, id, size, color, HttpContext.Session);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Decrease(int id, string size, string color)
        {
            var userId = HttpContext.Session.GetInt32("_UserId"); // Kiểm tra nếu đã đăng nhập
            _cartService.DecreaseQuantity(userId, id, size, color, HttpContext.Session);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int id, string size, string color)
        {
            var userId = HttpContext.Session.GetInt32("_UserId"); // Kiểm tra nếu đã đăng nhập
            _cartService.RemoveItem(userId, id, size, color, HttpContext.Session);
            TempData["Success"] = "Removed product";
            return RedirectToAction("Index");

        }
    }
}
