using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Models;

namespace PBL3_OnlineShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly PBL3_Db_Context _context;
        private readonly IPasswordHasher<User> _passwordHasher;


        public AccountController(PBL3_Db_Context context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        // GET: AccountController
        public ActionResult Index()
        {
            return View();
        }

        // Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string Password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == username);

            if (user == null)
            {
                ViewBag.Error = "Email không tồn tại.";
                return View();
            }

            // Kiểm tra mật khẩu đã hash
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, Password);

            if (result == PasswordVerificationResult.Success)
            {
                // Đăng nhập thành công, lưu thông tin người dùng vào session
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("FullName", user.FullName);

                return RedirectToAction("Index", "Home"); // Redirect to home page
            }

            // Nếu mật khẩu sai
            ViewBag.Error = "Sai mật khẩu.";
            return View();
        }


        // Login end

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(string FullName, string Email, string Password, string ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                return View();
            }

            var existingUser = _context.Users.FirstOrDefault(u => u.Email == Email);
            if (existingUser != null)
            {
                ViewBag.Error = "Email already exists.";
                return View();
            }

            var user = new User
            {
                FullName = FullName,
                Email = Email,
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, Password);
            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // GET: AccountController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
