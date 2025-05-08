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
        public async Task<IActionResult> Create(Products product, List<ProductSize> Sizes)
        {
            // Cập nhật danh sách các Category để hiển thị trong dropdown
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "CategoryName", product.CategoryId);

            // Kiểm tra tính hợp lệ của mô hình
            if (ModelState.IsValid)
            {
                // Gán giá trị thời gian tạo và cập nhật cho sản phẩm
                product.CreatedAt = DateTime.Now;
                product.UpdatedAt = DateTime.Now;
                product.Status = "Available"; // Trạng thái mặc định

                // Xử lý ảnh tải lên
                if (product.ImageUpload != null && product.ImageUpload.Count > 0)
                {
                    var imagePaths = new List<string>();
                    foreach (var file in product.ImageUpload)
                    {
                        if (file.Length > 0)
                        {
                            // Tạo tên file duy nhất
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagesProducts", fileName);
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            // Lưu đường dẫn ảnh
                            imagePaths.Add("~/imagesProducts/" + fileName);
                        }
                    }
                    // Lưu lại URL ảnh
                    product.ImageUrl = string.Join(",", imagePaths);
                }

                // Lưu sản phẩm vào cơ sở dữ liệu
                _context.Products.Add(product);
                await _context.SaveChangesAsync(); // Lưu sản phẩm

                // Lưu các size, color và quantity vào bảng ProductSizes
                if (Sizes != null && Sizes.Any())
                {
                    foreach (var size in Sizes)
                    {
                        size.ProductId = product.ProductId; // Gán ProductId cho từng ProductSize
                        _context.ProductsSize.Add(size); // Thêm từng ProductSize vào bảng ProductSizes
                    }
                    await _context.SaveChangesAsync(); // Lưu ProductSizes
                }

                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Thêm không thành công";
            }

            return View(product);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductSizes) // nạp thêm bảng liên quan
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Products product, List<ProductSize> Sizes)
        {
            // Cập nhật danh sách Category để hiển thị trong dropdown
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "CategoryName", product.CategoryId);

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Cập nhật không thành công";
                return View(product);
            }

            // Lấy sản phẩm hiện tại từ DB để xử lý ảnh và các giá trị không có trong form
            var existingProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            // Gán lại CreatedAt gốc (không thay đổi khi cập nhật)
            product.CreatedAt = existingProduct.CreatedAt;
            product.UpdatedAt = DateTime.Now;
            product.Status = "Available";

            // Gán lại ImageUrl cũ để xử lý nếu không upload ảnh mới
            product.ImageUrl = existingProduct.ImageUrl;

            // Xử lý ảnh mới
            if (product.ImageUpload != null && product.ImageUpload.Count > 0)
            {
                var imagePaths = new List<string>();

                foreach (var file in product.ImageUpload)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagesProducts", fileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        imagePaths.Add("~/imagesProducts/" + fileName);
                    }
                }

                // Xóa ảnh cũ
                if (!string.IsNullOrEmpty(existingProduct.ImageUrl))
                {
                    var oldImages = existingProduct.ImageUrl.Split(',');

                    foreach (var relativePath in oldImages)
                    {
                        var pathToDelete = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot",
                            relativePath.Replace("~/", "").Replace("/", Path.DirectorySeparatorChar.ToString())
                        );

                        if (System.IO.File.Exists(pathToDelete))
                        {
                            System.IO.File.Delete(pathToDelete);
                        }
                    }
                }

                // Cập nhật đường dẫn ảnh mới
                product.ImageUrl = string.Join(",", imagePaths);
            }

            // Cập nhật sản phẩm
            _context.Products.Update(product);

            // Xử lý cập nhật ProductSize
            var existingSizes = await _context.ProductsSize
                .Where(ps => ps.ProductId == product.ProductId)
                .ToListAsync();

            var sizesToDelete = existingSizes
                .Where(existingSize => !Sizes
                    .Any(newSize => newSize.Size == existingSize.Size && newSize.Color == existingSize.Color))
                .ToList();

            _context.ProductsSize.RemoveRange(sizesToDelete);

            foreach (var sizeColor in Sizes)
            {
                var existingSize = existingSizes
                    .FirstOrDefault(ps => ps.Size == sizeColor.Size && ps.Color == sizeColor.Color);

                if (existingSize != null)
                {
                    existingSize.Quantity = sizeColor.Quantity;
                    _context.ProductsSize.Update(existingSize);
                }
                else
                {
                    sizeColor.ProductId = product.ProductId;
                    _context.ProductsSize.Add(sizeColor);
                }
            }

            // Tính lại tổng tồn kho
            product.StockQuantity = Sizes.Sum(s => s.Quantity);

            // Lưu thay đổi
            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật sản phẩm thành công!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductSizes)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            // Xóa các ProductSize liên quan trước
            if (product.ProductSizes != null && product.ProductSizes.Any())
            {
                _context.ProductsSize.RemoveRange(product.ProductSizes);
            }

            // Xóa ảnh trong thư mục wwwroot/imagesProducts nếu có
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var imagePaths = product.ImageUrl.Split(',');
                foreach (var relativePath in imagePaths)
                {
                    var fullPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        relativePath.Replace("~/", "").Replace("/", Path.DirectorySeparatorChar.ToString())
                    );

                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }

            }

            // Xóa sản phẩm
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Xóa sản phẩm thành công.";
            return RedirectToAction(nameof(Index));
        }

    }
}
