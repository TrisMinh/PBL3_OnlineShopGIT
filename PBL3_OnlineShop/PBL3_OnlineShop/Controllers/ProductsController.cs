using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Bắt buộc để dùng Include
using PBL3_OnlineShop.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PBL3_OnlineShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly PBL3_Db_Context _context;

        public ProductsController(PBL3_Db_Context context)
        {
            _context = context;
        }

        // GET: ProductsController
        public ActionResult Index(string category, string color, string size, string price, string collection, string availability, string gender, string text, int page = 1)
        {
            var categories = new List<string> { "New", "Sales", "Polo Shirts", "Shorts", "Suits", "Best sellers", "T-Shirts", "Jeans", "Jackets", "Coats" };

            var query = _context.Products
                .Include(p => p.ProductSizes) // Để lọc theo Size/Color
                .Include(p => p.Category) // Nếu cần lọc theo category name
                .AsNoTracking() // Thêm vào để tối ưu truy vấn chỉ đọc
                .AsQueryable();

            if (!string.IsNullOrEmpty(category) && categories.Contains(category))
            {
                if (category == "New")
                {
                    var oneMonthAgo = DateTime.Now.AddMonths(-1);
                    query = query.Where(p => p.CreatedAt >= oneMonthAgo);
                }
                else if (category == "Sales")
                {
                    query = query.Where(p =>
                        p.SalePercentage != null && p.SalePercentage > 0 &&
                        p.Status != null && (
                            p.Status == "2" ||
                            p.Status.StartsWith("2,") ||
                            p.Status.EndsWith(",2") ||
                            p.Status.Contains(",2,")
                        )
                    );
                }
                else if (category == "Best sellers")
                {
                    query = query.Where(p => p.Status != null && (
                        p.Status == "3" ||
                        p.Status.StartsWith("3,") ||
                        p.Status.EndsWith(",3") ||
                        p.Status.Contains(",3,")
                    ));
                }
                else
                {
                    query = query.Where(p => p.Category != null && p.Category.CategoryName == category);
                }
            }

            if (!string.IsNullOrEmpty(color))
            {
                query = query.Where(p => p.ProductSizes.Any(ps => ps.Color.ToLower() == color.ToLower()));
            }

            if (!string.IsNullOrEmpty(size))
            {
                query = query.Where(p => p.ProductSizes.Any(ps => ps.Size.ToLower() == size.ToLower()));
            }

            if (!string.IsNullOrEmpty(price))
            {
                switch (price)
                {
                    case "under-500":
                        query = query.Where(p => p.SellingPrice < 500000);
                        break;
                    case "500-to-1m":
                        query = query.Where(p => p.SellingPrice >= 500000 && p.SellingPrice < 1000000);
                        break;
                    case "1m-to-2m":
                        query = query.Where(p => p.SellingPrice >= 1000000 && p.SellingPrice < 2000000);
                        break;
                    case "2m-to-4m":
                        query = query.Where(p => p.SellingPrice >= 2000000 && p.SellingPrice < 4000000);
                        break;
                    case "above-4m":
                        query = query.Where(p => p.SellingPrice >= 4000000);
                        break;
                }
            }

            if (!string.IsNullOrEmpty(collection))
            {
                query = query.Where(p => p.Collections != null && p.Collections.ToLower().Contains(collection.ToLower()));
            }

            if (!string.IsNullOrEmpty(availability))
            {
                if (availability == "available")
                {
                    query = query.Where(p => p.StockQuantity > 0);
                }
                else if (availability == "out-of-stock")
                {
                    query = query.Where(p => p.StockQuantity == 0);
                }
            }

            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(p => p.Gender != null && p.Gender.ToLower() == gender.ToLower());
            }

            if (!string.IsNullOrEmpty(text))
            {
                query = query.Where(p => p.ProductName.ToLower().Contains(text.ToLower()));
            }

            // Phân trang
            int pageSize = 30;
            int totalProducts = query.Count();
            int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
            var products = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = category;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SelectedCollection = collection;
            ViewBag.SelectedText = text;
            ViewBag.AvailableCount = query.Count(p => p.StockQuantity > 0);
            ViewBag.OutOfStockCount = query.Count(p => p.StockQuantity == 0);

            int? userId = HttpContext.Session.GetInt32("_UserId");
            if (userId == null)
            {
                Console.WriteLine("Chưa đăng nhập hoặc session không có UserId");
            }
            else
            {
                Console.WriteLine("UserId trong session: " + userId);
            }

            return View(products);
        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            var product = _context.Products
                .Include(p => p.ProductSizes)
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            // Lấy userId từ session
            int? userId = HttpContext.Session.GetInt32("_UserId");
            bool isFavourite = false;
            if (userId != null)
            {
                isFavourite = _context.Favourites.Any(f => f.UserId == userId && f.ProductId == id);
            }
            ViewBag.IsFavourite = isFavourite;

            return View(product);
        }

        [HttpGet]
        public IActionResult GetAvailableColors(int productId, string size)
        {
            if (string.IsNullOrEmpty(size))
                return Json(new List<string>());

            // Lấy từ database
            var colors = _context.ProductsSize
                .Where(ps => ps.ProductId == productId && ps.Size == size && ps.Quantity > 0)
                .Select(ps => ps.Color)
                .Distinct()
                .ToList();
            return Json(colors);
        }

    }
}
