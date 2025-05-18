using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Validation;
using PBL3_OnlineShop.Services.Admin.Category;

namespace PBL3_OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RoleAuthorization("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public IActionResult Index()
        {
            return View(_categoryService.GetAllCategories());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            var result = _categoryService.CreateCategory(category);
            if (!result)
            {
                TempData["Error"] = "Thêm danh mục không thành công!";
                return View(category);
                
            }
            TempData["Success"] = "Thêm danh mục thành công!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var categories = _categoryService.GetCategoryById(id);
            if (categories == null)
            {
                return NotFound();
            }
            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category category)
        {
            var result = _categoryService.UpdateCategory(category);
            if (!result)
            {
                TempData["Error"] = "Cập nhật danh mục không thành công!";
                return View(category);
            }

            TempData["Success"] = "Cập nhật danh mục thành công!";
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var result = _categoryService.DeleteCategory(id);
            if (!result)
            {
                TempData["Error"] = "Xóa danh mục không thành công!";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Xóa sản phẩm thành công.";
            return RedirectToAction("Index");
        }
    }
}
