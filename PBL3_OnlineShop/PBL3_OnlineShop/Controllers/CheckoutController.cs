using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using PBL3_OnlineShop.Migrations;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Repository;

namespace PBL3_OnlineShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly PBL3_Db_Context _context;
        public CheckoutController(PBL3_Db_Context context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("_UserId");
            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            var cartItem = _context.CartItems.Where(c => c.CartId == cart.CartId).ToList();
            return View(cartItem);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApplyCoupon(string name)
        {
            var userID = HttpContext.Session.GetInt32("_UserId");
            var coupon = _context.Coupons.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
            if (coupon == null)
            {
                TempData["Error"] = "Coupon not found.";
                return RedirectToAction("Index");
            }
            if (coupon.Quantity <= 0 || coupon.EndDate < DateTime.Now)
            {
                TempData["Error"] = "Coupon has expired.";
                return RedirectToAction("Index");
            }
            var couponUsage = _context.CouponUsages.Any(c => c.CouponId == coupon.Id && c.UserId == userID);
            if (couponUsage)
            {
                TempData["Error"] = "You have already used this coupon.";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Coupon " + coupon.Name + " applied successfully! " + coupon.Description;
            TempData["Discount"] = coupon.Discount.ToString();
            TempData["NameCoupon"] = coupon.Name;

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrder(decimal TotalPrice, string CouponUsed, List<CartItem> cartItems)
        {
            var userId = HttpContext.Session.GetInt32("_UserId");
            var order = new Order
            {
                UserId = userId.Value,
                OrderDate = DateTime.Now,
                Status = 1,
                TotalPrice = TotalPrice,
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.SellingPrice,
                };
                _context.OrderDetails.Add(orderDetail);
            }

            _context.SaveChanges();

            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            var cartItem = _context.CartItems.Where(c => c.CartId == cart.CartId).ToList();
            foreach (var item in cartItem)
            {
                _context.CartItems.Remove(item);
            }
            _context.SaveChanges();
            if (!string.IsNullOrEmpty(CouponUsed))
            {
                var coupon = _context.Coupons.FirstOrDefault(c => c.Name == CouponUsed);
                var couponUsage = new CouponUsage
                {
                    UserId = userId.Value,
                    CouponId = coupon.Id
                };

                _context.CouponUsages.Add(couponUsage);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
