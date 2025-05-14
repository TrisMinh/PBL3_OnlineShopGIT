using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Data;

namespace PBL3_OnlineShop.Services.Home
{
    public class HomeService: IHomeService
    {
        private readonly PBL3_Db_Context _context;
        public HomeService(PBL3_Db_Context context)
        {
            _context = context;
        }
        public List<Models.Product> GetHotProducts()
        {
            return _context.Products
                .Include(p => p.ProductSizes)  
                .Where(p => p.Status != null && (p.Status == "3" || p.Status.StartsWith("3,") || p.Status.EndsWith(",3") || p.Status.Contains(",3,")))
                .OrderByDescending(p => p.ProductId)
                .Take(8)
                .ToList();
        }
        public List<Models.Product> GetSaleProducts()
        {
            return _context.Products
                .Include(p => p.ProductSizes) 
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
        }
        public List<Models.Product> GetAllProducts()
        {
            return _context.Products
                .Include(p => p.ProductSizes)  // Bao gồm ProductSizes
                .OrderBy(p => p.ProductId)
                .Take(9)
                .ToList();
        }
    }
}
