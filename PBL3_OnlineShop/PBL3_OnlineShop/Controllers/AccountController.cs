using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Models.ViewModels;
using PBL3_OnlineShop.Data;

namespace PBL3_OnlineShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly PBL3_Db_Context _context;
        private readonly PasswordHasher<User> _passwordHasher = new(); 
        public AccountController(PBL3_Db_Context context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordView model)
        {
            // rỗng
            if (string.IsNullOrEmpty(model.ForgotUsername))
            {
                ModelState.AddModelError(string.Empty, "Username or email is required.");
                return View(model);
            }
            var user = _context.Users.FirstOrDefault(u => u.UserName == model.ForgotUsername || u.Email == model.ForgotUsername);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "No user found with the provided username or email.");
                return View(model);
            }
            var newPassword = "123";
            user.Password = _passwordHasher.HashPassword(user, newPassword);

            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }
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
                Gender = "Man",
                UrlAvatar = "/avatar/def.jpg",
                Role = "Customer",
                CreatedAt = DateTime.Now,
                Status = 1
            };
            // hash sau khi tạo vì tạo phía trong thì user ch đc khởi tạo
            user.Password = _passwordHasher.HashPassword(user, model.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đăng ký thành công!";
            return RedirectToAction("Login");
        }
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

                return RedirectToAction("Login");
            }
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                return NotFound();
            }
            
            // Xử lý các giá trị mặc định hoặc null
            if (user.Gender == null)
            {
                user.Gender = "Man"; // Giá trị mặc định
            }
            
            if (user.DateOfBirth == default(DateTime))
            {
                user.DateOfBirth = DateTime.Now; // Đặt ngày mặc định là ngày hiện tại
            }
            
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("_UserId");
                if (userId == null)
                {
                    return Unauthorized(new { success = false, message = "Bạn chưa đăng nhập" });
                }

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy người dùng" });
                }

                // Kiểm tra mật khẩu hiện tại
                var passwordCheck = _passwordHasher.VerifyHashedPassword(user, user.Password, model.CurrentPassword);
                if (passwordCheck == PasswordVerificationResult.Failed)
                {
                    return BadRequest(new { success = false, message = "Mật khẩu hiện tại không đúng" });
                }

                // Xác nhận mật khẩu mới
                if (model.NewPassword != model.ConfirmPassword)
                {
                    return BadRequest(new { success = false, message = "Mật khẩu mới và xác nhận mật khẩu không khớp" });
                }

                // Mã hóa và lưu mật khẩu mới
                user.Password = _passwordHasher.HashPassword(user, model.NewPassword);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Đổi mật khẩu thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(IFormCollection form)
        {
            Console.WriteLine("UpdateProfile method được gọi");
            try 
            {
                var userId = HttpContext.Session.GetInt32("_UserId");
                if (userId == null)
                {
                    Console.WriteLine("UserId không tồn tại trong session");
                    return RedirectToAction("Login", "Account");
                }
                
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    Console.WriteLine("Không tìm thấy user với ID: " + userId);
                    return NotFound();
                }
                
                // Debug các giá trị từ form
                foreach (var key in form.Keys)
                {
                    Console.WriteLine($"Form key: {key}, value: {form[key]}");
                }
                
                // Cập nhật thông tin người dùng từ form
                user.UserName = form["UserName"].ToString();
                user.Email = form["Email"].ToString();
                
                // Kiểm tra số điện thoại
                var phoneNumber = form["PhoneNumber"].ToString();
                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    if (phoneNumber.Length < 10 || phoneNumber.Length > 11)
                    {
                        Console.WriteLine("Số điện thoại không hợp lệ: " + phoneNumber);
                        return BadRequest("Số điện thoại phải từ 10 đến 11 số.");
                    }
                    user.PhoneNumber = phoneNumber;
                }
                
                // Lưu giới tính theo đúng giá trị đã chọn
                user.Gender = form["Gender"].ToString();
                Console.WriteLine("Gender value: " + user.Gender);
                
                // Xử lý ngày sinh
                if (int.TryParse(form["Day"], out int day) && 
                    int.TryParse(form["Month"], out int month) && 
                    int.TryParse(form["Year"], out int year))
                {
                    try
                    {
                        user.DateOfBirth = new DateTime(year, month, day);
                        Console.WriteLine($"Ngày sinh: {day}/{month}/{year}");
                    }
                    catch (Exception ex)
                    {
                        // Nếu ngày không hợp lệ, trả về lỗi
                        Console.WriteLine("Lỗi khi thiết lập ngày sinh: " + ex.Message);
                        return BadRequest("Ngày sinh không hợp lệ.");
                    }
                }
                else
                {
                    Console.WriteLine($"Lỗi parse ngày sinh: Day={form["Day"]}, Month={form["Month"]}, Year={form["Year"]}");
                }
                
                // Lưu địa chỉ (đã được định dạng từ client: tỉnh / huyện / địa chỉ cụ thể)
                user.Address = form["Address"].ToString();
                Console.WriteLine("Address value: " + user.Address);
                
                // Lưu thay đổi
                await _context.SaveChangesAsync();
                Console.WriteLine("Đã lưu thông tin người dùng thành công");
                
                // Cập nhật session nếu cần
                if (!string.IsNullOrEmpty(user.UserName))
                {
                    HttpContext.Session.SetString("_Username", user.UserName);
                }
                
                if (!string.IsNullOrEmpty(user.Email))
                {
                    HttpContext.Session.SetString("_Email", user.Email);
                }
                
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi xử lý UpdateProfile: " + ex.Message);
                return StatusCode(500, "Đã xảy ra lỗi khi cập nhật thông tin: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("_UserId");
                if (userId == null)
                {
                    return Unauthorized(new { success = false, message = "Bạn chưa đăng nhập" });
                }

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy người dùng" });
                }

                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { success = false, message = "Vui lòng chọn ảnh" });
                }

                // Kiểm tra loại file (chỉ cho phép ảnh)
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                string fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(new { success = false, message = "Chỉ chấp nhận định dạng JPG, JPEG, PNG hoặc GIF" });
                }

                // Tạo tên file duy nhất để tránh trùng lặp
                string uniqueFileName = $"{userId}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{fileExtension}";
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatar");
                
                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                // Lưu file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Xóa avatar cũ nếu có và không phải ảnh mặc định
                if (!string.IsNullOrEmpty(user.UrlAvatar) && !user.UrlAvatar.Contains("def"))
                {
                    string oldAvatarPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.UrlAvatar.TrimStart('/'));
                    if (System.IO.File.Exists(oldAvatarPath))
                    {
                        System.IO.File.Delete(oldAvatarPath);
                    }
                }

                // Cập nhật đường dẫn avatar trong database
                user.UrlAvatar = $"/avatar/{uniqueFileName}";
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Cập nhật avatar thành công", avatarUrl = user.UrlAvatar });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }
    }
}
