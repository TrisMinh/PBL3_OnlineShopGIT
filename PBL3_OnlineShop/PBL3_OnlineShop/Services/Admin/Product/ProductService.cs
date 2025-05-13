using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Models;

namespace PBL3_OnlineShop.Services.Admin.Product
{
    public class ProductService : IProductService
    {
        private readonly PBL3_Db_Context _context;

        public ProductService(PBL3_Db_Context context)
        {
            _context = context;
        }

        public List<Models.Product> GetAllProducts()
        {
            SyncStockQuantities();
            return _context.Products.Include(p => p.Category).Include(p => p.ProductSizes).OrderByDescending(p => p.ProductId).ToList();
        }

        public Models.Product GetProductById(int id)
        {
            return _context.Products.Include(p => p.ProductSizes).FirstOrDefault(p => p.ProductId == id);
        }

        public bool CreateProduct(Models.Product product, List<ProductSize> sizes)
        {
            if (_context.Products.Any(p => p.ProductName == product.ProductName))
            {
                return false;
            }

            product.CreatedAt = DateTime.Now;
            product.UpdatedAt = DateTime.Now;

            _context.Products.Add(product);
            _context.SaveChanges();

            var groupedSizes = sizes.GroupBy(s => new { s.Size, s.Color })
                .Select(g => new ProductSize
                {
                    Size = g.Key.Size,
                    Color = g.Key.Color,
                    Quantity = g.Sum(x => x.Quantity),
                    ProductId = product.ProductId
                }).ToList();

            if (groupedSizes.Any())
            {
                _context.ProductsSize.AddRange(groupedSizes);
                _context.SaveChanges();
                product.StockQuantity = groupedSizes.Sum(s => s.Quantity);
                _context.Products.Update(product);
                _context.SaveChanges();
            }
            return true;
        }

        public bool UpdateProduct(int id, Models.Product product, List<ProductSize> sizes)
        {
            var existingProduct = _context.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == id);
            if (existingProduct == null) return false;

            product.CreatedAt = existingProduct.CreatedAt;
            product.UpdatedAt = DateTime.Now;
            product.ImageUrl = existingProduct.ImageUrl;

            _context.Products.Update(product);

            var groupedSizes = sizes.GroupBy(s => new { s.Size, s.Color })
                .Select(g => new ProductSize
                {
                    Size = g.Key.Size,
                    Color = g.Key.Color,
                    Quantity = g.Sum(x => x.Quantity)
                }).ToList();

            var existingSizes = _context.ProductsSize.Where(ps => ps.ProductId == id).ToList();
            var toDelete = existingSizes.Where(e => !groupedSizes.Any(n => n.Size == e.Size && n.Color == e.Color)).ToList();

            _context.ProductsSize.RemoveRange(toDelete);

            foreach (var sizeColor in groupedSizes)
            {
                var existingSize = existingSizes.FirstOrDefault(e => e.Size == sizeColor.Size && e.Color == sizeColor.Color);
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

            product.StockQuantity = groupedSizes.Sum(s => s.Quantity);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteProduct(int id)
        {
            var product = _context.Products.Include(p => p.ProductSizes).FirstOrDefault(p => p.ProductId == id);
            if (product == null) return false;

            if (product.ProductSizes.Any()) _context.ProductsSize.RemoveRange(product.ProductSizes);
            _context.Products.Remove(product);
            _context.SaveChanges();
            return true;
        }

        public List<Models.Product> SearchProducts(int? productID, string productName, decimal? price, string category, string status, string gender)
        {
            var query = from p in _context.Products.Include(p => p.Category).Include(p => p.ProductSizes)
                        where (!productID.HasValue || p.ProductId == productID)
                           && (string.IsNullOrEmpty(productName) || p.ProductName.Contains(productName))
                           && (!price.HasValue || p.SellingPrice == price)
                           && (string.IsNullOrEmpty(category) || p.Category.CategoryName.Contains(category))
                           && (string.IsNullOrEmpty(status) || p.Status == status)
                           && (string.IsNullOrEmpty(gender) || p.Gender == gender)
                        select p;
            return query.ToList();
        }

        public void SyncStockQuantities()
        {
            var products = _context.Products.Include(p => p.ProductSizes).ToList();
            foreach (var product in products)
            {
                product.StockQuantity = product.ProductSizes?.Sum(ps => ps.Quantity) ?? 0;
            }
            _context.SaveChanges();
        }

        public List<Models.Category> GetCategorySelectList()
        {
            return _context.Categories.ToList();
        }
    }
}