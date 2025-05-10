using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Repository;

namespace PBL3_OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
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
            // Kiểm tra tính hợp lệ của model
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Cập nhật không thành công";
                return View(user);
            }

            // Lấy thông tin danh mục hiện tại từ DB
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existingUser == null)
            {
                return NotFound();
            }

            // Cập nhật các thuộc tính từ Category mới
            existingUser.UserName = user.UserName;
            existingUser.Password = _passwordHasher.HashPassword(user, user.Password);
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Address = user.Address;
            existingUser.DateOfBirth = user.DateOfBirth;
            existingUser.Role = user.Role;
            existingUser.Status = user.Status;

            _context.Users.Update(existingUser);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật user thành công!";
            return RedirectToAction("Index");
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
