using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Data;
using System.Diagnostics;

namespace PBL3_OnlineShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PBL3_Db_Context _context;

        public HomeController(ILogger<HomeController> logger, PBL3_Db_Context context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Lấy 8 sản phẩm Best sellers(hot products) và bao gồm ProductSizes
            var hotProducts = _context.Products
                .Include(p => p.ProductSizes)  // Bao gồm ProductSizes
                .Where(p => p.Status != null && (p.Status == "3" || p.Status.StartsWith("3,") || p.Status.EndsWith(",3") || p.Status.Contains(",3,")))
                .OrderByDescending(p => p.ProductId)
                .Take(8)
                .ToList();
            ViewBag.HotProducts = hotProducts;

            // Lấy 8 sản phẩm Sales (SalePercentage > 0 và Status chứa '2') và bao gồm ProductSizes
            var saleProducts = _context.Products
                .Include(p => p.ProductSizes)  // Bao gồm ProductSizes
                .Where(p => p.SalePercentage != null && p.SalePercentage > 0 &&
                    p.Status != null && (
                        p.Status == "2" ||
                        p.Status.StartsWith("2,") ||
                        p.Status.EndsWith(",2") ||
                        p.Status.Contains(",2,")
                    ))
                .OrderByDescending(p => p.SalePercentage)
                .Take(8)
                .ToList();
            ViewBag.SaleProducts = saleProducts;

            // Lấy 9 sản phẩm đầu tiên cho ALL COLLECTIONS (ProductId tăng dần) và bao gồm ProductSizes
            var allProducts = _context.Products
                .Include(p => p.ProductSizes)  // Bao gồm ProductSizes
                .OrderBy(p => p.ProductId)
                .Take(9)
                .ToList();
            ViewBag.AllProducts = allProducts;

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
