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


namespace PBL3_OnlineShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly PBL3_Db_Context _context;
        public CheckoutController(PBL3_Db_Context context)
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

            decimal shippingCost = 50;

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            var cartItems = _context.CartItems.Where(c => c.CartId == cart.CartId).ToList();

            decimal subtotal = cartItems.Sum(item => item.Quantity * item.SellingPrice);

            decimal discount = 0;
            string couponUsed = null;
            if (TempData["Discount"] != null)
            {
                decimal.TryParse(TempData["Discount"].ToString(), out discount);
                couponUsed = TempData["NameCoupon"]?.ToString();
            }

            decimal totalPrice = subtotal - discount + shippingCost;

            var checkoutViewModel = new CheckoutView
            {
                CartItems = cartItems,
                Subtotal = subtotal,
                ShippingCost = shippingCost,
                Discount = discount,
                TotalPrice = totalPrice,
                CouponUsed = couponUsed
            };

            ViewBag.NameCustomer = user.UserName;
            ViewBag.Email = user.Email;
            ViewBag.PhoneNumber = user.PhoneNumber;
            ViewBag.Address = user.Address;
            return View(checkoutViewModel);
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
        public IActionResult CreateOrder(string CouponUsed, string PaymentMethod)
        {
            var userId = HttpContext.Session.GetInt32("_UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            var cartItems = _context.CartItems.Where(c => c.CartId == cart.CartId).ToList();

            decimal subtotal = cartItems.Sum(item => item.Quantity * item.SellingPrice);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            decimal shippingCost = 50;
            decimal discount = 0;
            if (!string.IsNullOrEmpty(CouponUsed))
            {
                var coupon = _context.Coupons.FirstOrDefault(c => c.Name.ToLower() == CouponUsed.ToLower());
                discount = coupon.Discount;
            }

            decimal totalPrice = subtotal - discount + shippingCost;

            if (cartItems == null)
            {
                TempData["Error"] = "Cart items is null";
                return RedirectToAction("Index");
            }
            
            if (cartItems.Count == 0)
            {
                TempData["Error"] = "Not have products selected";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(PaymentMethod))
            {
                TempData["Error"] = "Vui lòng chọn phương thức thanh toán.";
                return RedirectToAction("Index");
            }

            var productIds = new HashSet<int>();
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
                    productIds.Add(item.ProductId);
                }
                else
                {
                    TempData["Error"] = "Product not found in stock." + item.ProductId + " " + item.ProductName + " Color: " + item.Color + " Size: " + item.Size;
                    return RedirectToAction("Index");
                }
            }

            // Nếu là Credit Card, lưu thông tin vào TempData và chuyển hướng đến trang Payment
            if (PaymentMethod == "CreditCard")
            {
                // Lưu thông tin cần thiết vào TempData
                TempData["TotalPrice"] = totalPrice.ToString();
                TempData["CouponUsed"] = CouponUsed;
                
                // Chuyển hướng đến trang Payment tạm thời
                return RedirectToAction("PrePayment");
            }
            
            // Nếu là COD, tiếp tục quy trình lưu đơn hàng với Code = "0"
            var order = CreateOrderInDatabase(totalPrice, CouponUsed, cartItems, userId.Value, productIds, "0");
            
            TempData["Success"] = "Order placed successfully!";
            return RedirectToAction("Index", "Order");
        }
        
        // Phương thức tạo đơn hàng trong database
        private Order CreateOrderInDatabase(decimal TotalPrice, string CouponUsed, List<CartItem> cartItems, int userId, HashSet<int> productIds, string randomCode = null)
        {
            foreach (var item in cartItems)
            {
                var productSize = _context.ProductsSize.FirstOrDefault(ps => ps.ProductId == item.ProductId && ps.Color == item.Color && ps.Size == item.Size);
                if (productSize != null)
                {
                    productSize.Quantity -= item.Quantity;
                    _context.ProductsSize.Update(productSize);
                }
            }

            var order = new Order
            {
                UserId = userId,
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
                    Code = randomCode ?? "0" // Nếu là COD, lưu "0", ngược lại lưu mã ngẫu nhiên
                };
                _context.OrderDetails.Add(orderDetail);
            }

            _context.SaveChanges();

            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart != null)
            {
                foreach (var item in cartItems)
                {
                    var cartItemToRemove = _context.CartItems.FirstOrDefault(c => 
                        c.CartId == cart.CartId && 
                        c.ProductId == item.ProductId && 
                        c.Size == item.Size && 
                        c.Color == item.Color);
                    
                    if (cartItemToRemove != null)
                    {
                        _context.CartItems.Remove(cartItemToRemove);
                    }
                }
                _context.SaveChanges();
            }
            
            UpdateProductsStockQuantity(productIds);
            
            if (!string.IsNullOrEmpty(CouponUsed))
            {
                var coupon = _context.Coupons.FirstOrDefault(c => c.Name == CouponUsed);
                if (coupon != null)
                {
                    var couponUsage = new CouponUsage
                    {
                        UserId = userId,
                        CouponId = coupon.Id
                    };

                    _context.CouponUsages.Add(couponUsage);
                    coupon.Quantity -= 1;
                    _context.Coupons.Update(coupon);
                    _context.SaveChanges();
                }
            }
            
            return order;
        }
        
        // Phương thức để cập nhật StockQuantity trong Products dựa trên số lượng trong ProductsSize
        private void UpdateProductsStockQuantity(HashSet<int> productIds)
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
        
        // Phương thức mới để hiển thị trang trước khi thanh toán
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
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            
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

            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            var cartItems = _context.CartItems.Where(c => c.CartId == cart.CartId).ToList();

            // Tạo danh sách productIds để cập nhật StockQuantity
            var productIds = new HashSet<int>();
            foreach (var item in cartItems)
            {
                productIds.Add(item.ProductId);
            }
            
            // Tạo đơn hàng trong cơ sở dữ liệu
            var order = CreateOrderInDatabase(totalPrice, couponUsed, cartItems, userId.Value, productIds, randomCode);
            
            TempData["Success"] = "Payment completed successfully!";
            return RedirectToAction("Index", "Order");
        }
    }
}
