using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using PBL3_OnlineShop.Migrations;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Data;
using System.Text.Json;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Models.ViewModels;
using PBL3_OnlineShop.Services;
using PBL3_OnlineShop.Services.Checkout;


namespace PBL3_OnlineShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICheckoutService _checkoutService;
        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("_UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            string couponUsed = TempData["NameCoupon"]?.ToString();

            CheckoutView checkoutView = _checkoutService.GetCheckoutView(userId, couponUsed);

            var user = _checkoutService.GetUserById(userId);

            ViewBag.NameCustomer = user.UserName;
            ViewBag.Email = user.Email;
            ViewBag.PhoneNumber = user.PhoneNumber;
            ViewBag.Address = user.Address;
            return View(checkoutView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApplyCoupon(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var userID = HttpContext.Session.GetInt32("_UserId");

                if (_checkoutService.CheckCoupon(userID, name) != "OK")
                {
                    TempData["Error"] = _checkoutService.CheckCoupon(userID, name);
                    return RedirectToAction("Index");
                }

                var coupon = _checkoutService.GetCouponByName(name);
                TempData["OK"] = "Coupon " + coupon.Name + " applied successfully! " + coupon.Description;
                TempData["NameCoupon"] = coupon.Name;

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrder(string CouponUsed, string PaymentMethod)
        {
            var userId = HttpContext.Session.GetInt32("_UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrWhiteSpace(PaymentMethod))
            {
                TempData["Error"] = "Vui lòng chọn phương thức thanh toán.";
                return RedirectToAction("Index");
            }

            var cart = _checkoutService.GetCartByUserId(userId);
            var cartItems = _checkoutService.GetListCartItemsByCartId(cart.CartId);

            if (_checkoutService.CheckProductSize(cartItems) != "OK")
            {
                TempData["Error"] = _checkoutService.CheckProductSize(cartItems);
                return RedirectToAction("Index");
            }

            decimal totalPrice = _checkoutService.CaculateTotalPrice(userId, CouponUsed);
            if (PaymentMethod == "CreditCard")
            {
                TempData["TotalPrice"] = totalPrice.ToString();
                TempData["CouponUsed"] = CouponUsed;

                return RedirectToAction("PrePayment");
            }

            var productIds = _checkoutService.GetListProductIdFromCartItems(cartItems, cart.CartId);

            _checkoutService.CreateOrderInDatabase(totalPrice, CouponUsed, cartItems, userId.Value, productIds, "0");

            TempData["Success"] = "Order placed successfully!";
            return RedirectToAction("Index", "Order");
        }

        public IActionResult PrePayment()
        {
            var userId = HttpContext.Session.GetInt32("_UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (TempData["TotalPrice"] == null)
            {
                TempData["Error"] = "Payment information is missing.";
                return RedirectToAction("Index", "Checkout");
            }

            // Lấy thông tin người dùng
            var user = _checkoutService.GetUserById(userId);

            // Tạo đối tượng Order tạm thời để hiển thị
            var order = new Order
            {
                User = user,
                TotalPrice = decimal.Parse(TempData["TotalPrice"].ToString()),
                OrderDate = DateTime.Now
            };

            // Giữ dữ liệu trong TempData để sử dụng sau này
            TempData.Keep("CartItems");
            TempData.Keep("TotalPrice");
            TempData.Keep("CouponUsed");

            return View("Payment", order);
        }

        // Phương thức xử lý thanh toán hoàn tất đã được sửa đổi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CompletePayment(string randomCode)
        {
            var userId = HttpContext.Session.GetInt32("_UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy dữ liệu từ TempData
            if (TempData["TotalPrice"] == null)
            {
                TempData["Error"] = "Payment information is missing.";
                return RedirectToAction("Index", "Checkout");
            }

            decimal totalPrice = decimal.Parse(TempData["TotalPrice"].ToString());
            string couponUsed = TempData["CouponUsed"]?.ToString();

            var cart = _checkoutService.GetCartByUserId(userId);
            var cartItems = _checkoutService.GetListCartItemsByCartId(cart.CartId);

            // Tạo danh sách productIds để cập nhật StockQuantity
            var productIds = new List<int>();
            foreach (var item in cartItems)
            {
                productIds.Add(item.ProductId);
            }

            // Tạo đơn hàng trong cơ sở dữ liệu
            _checkoutService.CreateOrderInDatabase(totalPrice, couponUsed, cartItems, userId.Value, productIds, randomCode);

            TempData["Success"] = "Payment completed successfully!";
            return RedirectToAction("Index", "Order");
        }
    }
}
