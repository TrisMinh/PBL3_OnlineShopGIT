using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Models.ViewModels;
using PBL3_OnlineShop.Repository;

namespace PBL3_OnlineShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly PBL3_Db_Context _context;
        private readonly PasswordHasher<User> _passwordHasher = new(); // dịch vụ mã hoá
        public AccountController(PBL3_Db_Context context)
        {
            _context = context;
        }
        // Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            bool isUserNameExists = await _context.Users.AnyAsync(u => u.UserName == model.UserName);
            if (isUserNameExists)
            {
                ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại.");
                return View(model);
            }
            bool isEmailExists = await _context.Users.AnyAsync(u => u.Email == model.Email);
            if (isEmailExists)
            {
                ModelState.AddModelError("Email", "Email đã tồn tại.");
                return View(model);
            }
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
                Role = "Customer",
                Status = 1
            };
            // hash sau khi tạo vì tạo phía trong thì user ch đc khởi tạo
            user.Password = _passwordHasher.HashPassword(user, model.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đăng ký thành công!";
            return RedirectToAction("Login");
        }
        // Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.Username);
            if (user == null)
            {
                ModelState.AddModelError("Username", "Tên đăng nhập không tồn tại.");
                return View(model);
            }
            var pass = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
            if (pass == PasswordVerificationResult.Failed) // passverificationresult g so sánh pass
            {
                ModelState.AddModelError("Password", "Mật khẩu không đúng.");
                return View(model);
            }
            // set session
            HttpContext.Session.SetInt32("_UserId", user.Id);
            HttpContext.Session.SetString("_Username", user.UserName);
            HttpContext.Session.SetString("_Role", user.Role);
            HttpContext.Session.SetString("_Email", user.Email);

            var tempCart = HttpContext.Session.GetString("Cart");
            if (!string.IsNullOrEmpty(tempCart))
            {
                var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(tempCart); // conver từ json về list<CartItem>
                var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == user.Id);

                if (cart == null)
                {
                    cart = new Cart { UserId = user.Id };
                    _context.Carts.Add(cart);
                }

                foreach (var item in cartItems)
                {
                    var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == item.ProductId && ci.Size == item.Size && ci.Color == item.Color);
                    if (existingItem != null)
                    {
                        existingItem.Quantity += item.Quantity;  // Cập nhật số lượng nếu sản phẩm đã có trong giỏ
                    }
                    else
                    {
                        cart.CartItems.Add(item);
                    }
                }

                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("Cart");  // Xóa giỏ hàng tạm thời sau khi đã chuyển vào cơ sở dữ liệu
            }

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Logout()
        {
            // Clear authentication cookie or session here
            HttpContext.Session.Clear(); // clear session
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("_UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
    }
}
