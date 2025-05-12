using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Validation;

namespace PBL3_OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RoleAuthorization("Admin")]
    public class UserController : Controller
    {      
        private readonly PBL3_Db_Context _context;
        private readonly PasswordHasher<User> _passwordHasher = new(); // dịch vụ mã hoá
        public UserController(PBL3_Db_Context context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.OrderByDescending(p => p.Id).ToListAsync());
        }
        public IActionResult Create()
        {
            ViewBag.RoleList = new List<SelectListItem>
            {
                new SelectListItem { Text = "Customer", Value = "Customer" },
                new SelectListItem { Text = "Admin", Value = "Admin" }
            };
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            
            if (ModelState.IsValid)
            {
                user.Password = _passwordHasher.HashPassword(user, user.Password);
                _context.Users.Add(user);
                await _context.SaveChangesAsync(); // Lưu để lấy ProductId
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Thêm không thành công";
            }

            return View(user);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users.FindAsync(id);
            ViewBag.RoleList = new List<SelectListItem>
            {
                new SelectListItem { Text = "Customer", Value = "Customer" },
                new SelectListItem { Text = "Admin", Value = "Admin" }
            };
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            try
            {
                ViewBag.RoleList = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Customer", Value = "Customer" },
                    new SelectListItem { Text = "Admin", Value = "Admin" }
                };

                // Lấy thông tin user hiện tại từ DB trước
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (existingUser == null)
                {
                    return NotFound();
                }

                // Lấy dữ liệu address từ form (nếu có)
                var province = Request.Form["province"].ToString();
                var district = Request.Form["district"].ToString();
                var specificAddress = Request.Form["specific-address"].ToString();

                if (!string.IsNullOrEmpty(province) && !string.IsNullOrEmpty(district))
                {
                    // Cập nhật address mới
                    existingUser.Address = province + " / " + district + " / " + specificAddress;
                }

                // Chỉ cập nhật các trường được gửi từ form
                if (!string.IsNullOrEmpty(user.UserName))
                {
                    existingUser.UserName = user.UserName;
                }
                // (Giữ nguyên password cũ, không cập nhật)
                if (!string.IsNullOrEmpty(user.Email))
                {
                    existingUser.Email = user.Email;
                }
                if (!string.IsNullOrEmpty(user.PhoneNumber))
                {
                    existingUser.PhoneNumber = user.PhoneNumber;
                }
                if (user.DateOfBirth != DateTime.MinValue)
                {
                    existingUser.DateOfBirth = user.DateOfBirth;
                }
                if (!string.IsNullOrEmpty(user.Role))
                {
                    existingUser.Role = user.Role;
                }
                existingUser.Status = user.Status;

                // Cập nhật User trong DB
                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Cập nhật user thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi chi tiết
                TempData["Error"] = "Lỗi: " + ex.Message;
                if (ex.InnerException != null)
                {
                    TempData["Error"] += " - " + ex.InnerException.Message;
                }
                return View(user);
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existingUser == null)
            {
                return NotFound();
            }
            // Xóa sản phẩm
            _context.Users.Remove(existingUser);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Xóa user thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}
