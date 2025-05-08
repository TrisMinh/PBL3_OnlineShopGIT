using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Repository;

namespace PBL3_OnlineShop.Controllers
{
    public class AdminController : Controller
    {
        private readonly PBL3_Db_Context _context;

        public AdminController(PBL3_Db_Context context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products
            .OrderByDescending(p => p.ProductId)
            .Include(p => p.Category)   // Bao gồm Category
            .Include(p => p.ProductSizes) // Bao gồm ProductSizes
            .ToListAsync());
        }
        public async Task<IActionResult> UpdateStockQuantities()
        {
            var products = await _context.Products
                .Include(p => p.ProductSizes)
                .ToListAsync();

            foreach (var product in products)
            {
                product.StockQuantity = product.ProductSizes?.Sum(ps => ps.Quantity) ?? 0;
            }

            await _context.SaveChangesAsync();
            return Content("Stock quantities updated successfully.");
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "CategoryName"); // Lấy danh sách Category từ DbContext
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products product)
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "CategoryName", product.CategoryId);
            if (ModelState.IsValid)
            {
                product.CreatedAt = DateTime.Now;
                product.UpdatedAt = DateTime.Now;
                product.Status = "Available"; // Hoặc trạng thái mặc định khác
                if (product.ImageUpload != null && product.ImageUpload.Count > 0)
                {
                    var imagePaths = new List<string>();
                    foreach (var file in product.ImageUpload)
                    {
                        if (file.Length > 0)
                        {
                            // Tạo tên file duy nhất
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            // Đường dẫn lưu file (ví dụ wwwroot/images)
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagesProducts", fileName);
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            // Lưu lại đường dẫn (ví dụ: chỉ tên file hoặc kèm `/images/`)
                            imagePaths.Add("~/imagesProducts/" + fileName);
                        }
                    }

                    // Gán vào thuộc tính ImageURL, cách nhau bởi dấu phẩy
                    product.ImageUrl = string.Join(",", imagePaths);
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Thêm không thành công";
            }
            return View(product);
        }
    }
}
