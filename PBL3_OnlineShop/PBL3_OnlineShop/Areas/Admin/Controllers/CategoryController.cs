using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Repository;
using PBL3_OnlineShop.Validation;

namespace PBL3_OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RoleAuthorization("Admin")]
    public class CategoryController : Controller
    {
        private readonly PBL3_Db_Context _context;

        public CategoryController(PBL3_Db_Context context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories
            .OrderByDescending(p => p.CategoryId)
            .ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Status = 1;
                // Thêm sản phẩm
                _context.Categories.Add(category);
                await _context.SaveChangesAsync(); // Lưu để lấy ProductId
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Thêm không thành công";
            }

            return View(category);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var categories = await _context.Categories.FindAsync(id);

            if (categories == null)
            {
                return NotFound();
            }
            return View(categories);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            // Kiểm tra tính hợp lệ của model
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Cập nhật không thành công";
                return View(category);
            }

            // Lấy thông tin danh mục hiện tại từ DB
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (existingCategory == null)
            {
                return NotFound();
            }

            // Cập nhật các thuộc tính từ Category mới
            existingCategory.CategoryName = category.CategoryName;
            existingCategory.Description = category.Description;
            existingCategory.Status = category.Status;

            // Cập nhật Category trong cơ sở dữ liệu
            _context.Categories.Update(existingCategory);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật danh mục thành công!";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (existingCategory == null)
            {
                return NotFound();
            }
            // Xóa sản phẩm
            _context.Categories.Remove(existingCategory);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Xóa sản phẩm thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}
