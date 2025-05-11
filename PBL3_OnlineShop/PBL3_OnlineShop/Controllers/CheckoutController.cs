using Azure.Core;
using Microsoft.AspNetCore.Mvc;
//using PBL3_OnlineShop.Migrations;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Repository;
using System.Text.Json;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace PBL3_OnlineShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly PBL3_Db_Context _context;
        public CheckoutController(PBL3_Db_Context context)
        {
            _context = context;
        }
        
        private Dictionary<string, string> GetProvinces()
        {
            var provinces = new Dictionary<string, string>();
            try
            {
                string jsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "js", "vn-address.js");
                string content = System.IO.File.ReadAllText(jsFilePath);
                
                // Dùng regex để trích xuất mã và tên tỉnh
                var regex = new Regex(@"\{\s*code:\s*""(\d+)"",\s*name:\s*""([^""]+)""\s*\}");
                var matches = regex.Matches(content);
                
                foreach (Match match in matches)
                {
                    if (match.Groups.Count >= 3)
                    {
                        string code = match.Groups[1].Value;
                        string name = match.Groups[2].Value;
                        if (!provinces.ContainsKey(code))
                        {
                            provinces.Add(code, name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
            }
            return provinces;
        }
        
        private Dictionary<string, string> GetDistricts(string provinceCode)
        {
            var districts = new Dictionary<string, string>();
            try
            {
                string jsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "js", "vn-address.js");
                string content = System.IO.File.ReadAllText(jsFilePath);
                
                // Định vị khu vực chứa quận/huyện cho tỉnh cụ thể
                string pattern = $"\"{provinceCode}\":\\s*\\[(.*?)\\]";
                var match = Regex.Match(content, pattern, RegexOptions.Singleline);
                
                if (match.Success && match.Groups.Count >= 2)
                {
                    string districtsContent = match.Groups[1].Value;
                    var districtRegex = new Regex(@"\{\s*code:\s*""(\d+)"",\s*name:\s*""([^""]+)""\s*\}");
                    var matches = districtRegex.Matches(districtsContent);
                    
                    foreach (Match districtMatch in matches)
                    {
                        if (districtMatch.Groups.Count >= 3)
                        {
                            string code = districtMatch.Groups[1].Value;
                            string name = districtMatch.Groups[2].Value;
                            if (!districts.ContainsKey(code))
                            {
                                districts.Add(code, name);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
            }
            return districts;
        }
        
        public ActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("_UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.PaymentMethodList = new List<SelectListItem>
            {
                new SelectListItem { Text = "COD", Value = "COD" }, 
                new SelectListItem { Text = "Credit Card", Value = "CreditCard" } 
            };

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                ViewBag.UserName = user.Name;
                ViewBag.Email = user.Email;
                ViewBag.PhoneNumber = user.PhoneNumber;

                // Phân tích địa chỉ thành các phần
                if (!string.IsNullOrEmpty(user.Address))
                {
                    var addressParts = user.Address.Split('/');
                    if (addressParts.Length >= 3)
                    {
                        string provinceCode = addressParts[0].Trim();
                        string districtCode = addressParts[1].Trim();
                        string detailAddress = addressParts[2].Trim();

                        // Lấy tên tỉnh/thành phố từ mã
                        var provinces = GetProvinces();
                        if (provinces.ContainsKey(provinceCode))
                        {
                            ViewBag.Province = provinces[provinceCode];

                            // Lấy tên quận/huyện từ mã
                            var districts = GetDistricts(provinceCode);
                            if (districts.ContainsKey(districtCode))
                            {
                                ViewBag.District = districts[districtCode];
                            }
                            else
                            {
                                ViewBag.District = districtCode;
                            }
                        }
                        else
                        {
                            ViewBag.Province = provinceCode;
                        }

                        ViewBag.AddressDetail = detailAddress;
                    }
                    else
                    {
                        // Nếu địa chỉ không đúng định dạng, giữ nguyên
                        ViewBag.Address = user.Address;
                    }
                }
            }

            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            var cartItem = _context.CartItems.Where(c => c.CartId == cart.CartId).ToList();
            ViewBag.NameCustomer = user.Name;
            ViewBag.Email = user.Email;
            ViewBag.PhoneNumber = user.PhoneNumber;
            ViewBag.Address = user.Address;

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
        public IActionResult CreateOrder(decimal TotalPrice, string CouponUsed, List<CartItem> cartItems, string PaymentMethod)
        {
            var userId = HttpContext.Session.GetInt32("_UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Kiểm tra xem giỏ hàng có trống không - log để debug
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

            // Kiểm tra phương thức thanh toán
            if (string.IsNullOrWhiteSpace(PaymentMethod))
            {
                TempData["Error"] = "Vui lòng chọn phương thức thanh toán.";
                return RedirectToAction("Index");
            }

            // Kiểm tra số lượng tồn kho
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
                TempData["TotalPrice"] = TotalPrice.ToString();
                TempData["CouponUsed"] = CouponUsed;
                
                // Lưu danh sách CartItems vào TempData
                var cartItemsJson = System.Text.Json.JsonSerializer.Serialize(cartItems);
                TempData["CartItems"] = cartItemsJson;
                
                // Chuyển hướng đến trang Payment tạm thời
                return RedirectToAction("PrePayment");
            }
            
            // Nếu là COD, tiếp tục quy trình lưu đơn hàng với Code = "0"
            var order = CreateOrderInDatabase(TotalPrice, CouponUsed, cartItems, userId.Value, productIds, "0");
            
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

            // Chỉ xóa những sản phẩm đã đặt hàng khỏi giỏ hàng
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
            
            // Cập nhật StockQuantity trong Products
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
            
            if (TempData["CartItems"] == null || TempData["TotalPrice"] == null)
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
            if (TempData["CartItems"] == null || TempData["TotalPrice"] == null)
            {
                TempData["Error"] = "Payment information is missing.";
                return RedirectToAction("Index", "Checkout");
            }
            
            decimal totalPrice = decimal.Parse(TempData["TotalPrice"].ToString());
            string couponUsed = TempData["CouponUsed"]?.ToString();
            string cartItemsJson = TempData["CartItems"].ToString();
            
            // Deserialize danh sách CartItems
            var cartItems = System.Text.Json.JsonSerializer.Deserialize<List<CartItem>>(cartItemsJson);
            
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
