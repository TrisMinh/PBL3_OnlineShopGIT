using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Repository;

namespace PBL3_OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly PBL3_Db_Context _context;

        public ProductsController(PBL3_Db_Context context)
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

    }
}
