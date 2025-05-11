using Azure.Core;
using Microsoft.AspNetCore.Mvc;
//using PBL3_OnlineShop.Migrations;
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
            if (coupon == null || coupon.status == 0)
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

            foreach (var item in cartItems)
            {
                var productSize = _context.ProductsSize.FirstOrDefault(ps => ps.ProductId == item.ProductId && ps.Color == item.Color && ps.Size == item.Size);
                if (productSize != null)
                {
                    if (productSize.Quantity < item.Quantity)
                    {
                        TempData["Error"] = "Not enough stock for " + item.ProductName + " Color: " + item.Color + " Size: " + item.Size;
                        return RedirectToAction("Index");
                    }
                    productSize.Quantity -= item.Quantity;
                    _context.ProductsSize.Update(productSize);
                }
                else
                {
                    TempData["Error"] = "Product not found in stock." + item.ProductId + " " +item.ProductName + " Color: " + item.Color + " Size: " + item.Size;
                    return RedirectToAction("Index");
                }
            }

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
                    Size = item.Size,
                    Color = item.Color,
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
                coupon.Quantity -= 1;
                _context.Coupons.Update(coupon);
                _context.SaveChanges();
            }

            TempData["Success"] = "Order placed successfully!";
            return RedirectToAction("Index", "Home");
        }
    }
}
