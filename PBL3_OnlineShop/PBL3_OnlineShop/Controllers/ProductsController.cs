using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Models;
using System.Collections.Generic; // Required for List
using System.Linq; // Required for FirstOrDefault, Contains
using PBL3_OnlineShop.Data;

namespace PBL3_OnlineShop.Controllers
{
    public class ProductsController : Controller
    {
        // GET: ProductsController
        public ActionResult Index(string category, string color, string size, string price, string collection, string availability, string gender, string text, int page = 1)
        {
            var categories = new List<string> { "New", "Sales", "Polo Shirts", "Shorts", "Suits", "Best sellers", "T-Shirts", "Jeans", "Jackets", "Coats" };

            var query = _context.Products.AsQueryable();
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
                    query = query.Where(p => p.Category.CategoryName == category);
                }
            }
            if (!string.IsNullOrEmpty(color))
            {
                query = query.Where(p => p.Colors != null && p.Colors.ToLower().Contains(color.ToLower()));
            }
            if (!string.IsNullOrEmpty(size))
            {
                query = query.Where(p => p.Size != null && p.Size.ToLower().Contains(size.ToLower()));
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
            // Lọc theo tên sản phẩm nếu có text search
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

            return View(products);
        }

        // GET: ProductController/Details/5
        public  ActionResult Details(int id = 1)
        {
            if (id == 0) return View();

            var productbyId = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (productbyId == null)
            {
                return RedirectToAction("Index");
            }

            return View(productbyId);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

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

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // Original code
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // Original code
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
