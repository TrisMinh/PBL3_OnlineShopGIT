using Azure.Core;
using Microsoft.AspNetCore.Mvc;
//using PBL3_OnlineShop.Migrations;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Repository;
using System.Text.Json;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Rendering;

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

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

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
            return RedirectToAction("Index", "Order");
        }
    }
}
