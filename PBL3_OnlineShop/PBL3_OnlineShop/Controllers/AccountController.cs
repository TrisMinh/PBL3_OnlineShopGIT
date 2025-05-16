using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Models.ViewModels;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Services.Account;

namespace PBL3_OnlineShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordView model)
        {
            if (string.IsNullOrEmpty(model.ForgotUsername))
            {
                ModelState.AddModelError(string.Empty, "Username or email is required.");
                return View(model);
            }
            var user = _accountService.FindByUsernameOrEmail(model.ForgotUsername);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "No user found with the provided username or email.");
                return View(model);
            }
            _accountService.ResetPassword(user, "123");
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            if (_accountService.IsUserNameExists(model.UserName))
            {
                ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại.");
                return View(model);
            }

            if (_accountService.IsEmailExists(model.Email))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại.");
                return View(model);
            }

            _accountService.CreateUser(model);

            TempData["Success"] = "Đăng ký thành công!";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _accountService.FindByUsername(model.Username);
            if (user == null)
            {
                ModelState.AddModelError("Username", "Tên đăng nhập không tồn tại.");
                return View(model);
            }

            if (!_accountService.VerifyPassword(user, model.Password))
            {
                ModelState.AddModelError("Password", "Mật khẩu không đúng.");
                return View(model);
            }

            // Thiết lập session
            HttpContext.Session.SetInt32("_UserId", user.Id);
            HttpContext.Session.SetString("_Username", user.UserName);
            HttpContext.Session.SetString("_Role", user.Role);
            HttpContext.Session.SetString("_Email", user.Email);

            // Xử lý giỏ hàng
            var tempCart = HttpContext.Session.GetString("Cart");
            if (!string.IsNullOrEmpty(tempCart))
            {
                _accountService.ApplySessionCartToUser(tempCart, user.Id, HttpContext.Session);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
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
            
            var user = _accountService.GetUserById(userId.Value);
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
        public IActionResult UpdateProfile(string UserName, string Name, string Email, string PhoneNumber, string Gender, int Day, int Month, int Year, string Address)
        {
            var userId = HttpContext.Session.GetInt32("_UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            
            if (!ModelState.IsValid)
            {
                var user = _accountService.GetUserById(userId.Value);
                return View("Profile", user);
            }
            
            bool result = _accountService.UpdateProfile(userId.Value, UserName, Email, PhoneNumber, Gender, Day, Month, Year, Address);
            
            if (!result)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi khi cập nhật thông tin.");
                var user = _accountService.GetUserById(userId.Value);
                return View("Profile", user);
            }
            
            // Cập nhật session
            HttpContext.Session.SetString("_Username", UserName);
            HttpContext.Session.SetString("_Email", Email);
            
            // Thông báo thành công
            TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
            
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public IActionResult ChangePasswordForm(string CurrentPassword, string NewPassword, string ConfirmPassword)
        {
            var userId = HttpContext.Session.GetInt32("_UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            
            // Kiểm tra mật khẩu mới và xác nhận mật khẩu
            if (NewPassword != ConfirmPassword)
            {
                TempData["PasswordError"] = "Mật khẩu mới và xác nhận mật khẩu không khớp.";
                return RedirectToAction("Profile");
            }
            
            // Kiểm tra độ dài mật khẩu
            if (NewPassword.Length < 5)
            {
                TempData["PasswordError"] = "Mật khẩu mới phải có ít nhất 5 ký tự.";
                return RedirectToAction("Profile");
            }
            
            // Gọi service để thay đổi mật khẩu
            bool result = _accountService.ChangePassword(userId.Value, CurrentPassword, NewPassword);
            
            if (!result)
            {
                TempData["PasswordError"] = "Mật khẩu hiện tại không đúng hoặc đã xảy ra lỗi.";
                return RedirectToAction("Profile");
            }
            
            TempData["PasswordSuccess"] = "Đổi mật khẩu thành công!";
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> UploadAvatarForm(IFormFile file)
        {
            var userId = HttpContext.Session.GetInt32("_UserId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "Bạn chưa đăng nhập";
                return RedirectToAction("Login");
            }

            var result = await _accountService.UploadAvatar(userId.Value, file);
            
            if (result.success)
            {
                TempData["AvatarSuccess"] = result.message;
            }
            else
            {
                TempData["ErrorMessage"] = result.message;
            }
            
            return RedirectToAction("Profile");
        }
    }
}
