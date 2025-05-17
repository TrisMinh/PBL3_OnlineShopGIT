using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PBL3_OnlineShop.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly PBL3_Db_Context _context;

        public ProductService(PBL3_Db_Context context)
        {
            _context = context;
        }

        public List<Models.Product> GetProducts(string category, string color, string size, string price,
            string collection, string availability, string gender, string text, int page, int pageSize,
            out int totalPages, out int availableCount, out int outOfStockCount)
        {
            var categories = new List<string> { "New", "Sales", "Polo Shirts", "Shorts", "Suits", "Best sellers", "T-Shirts", "Jeans", "Jackets", "Coats" };

            var query = _context.Products
                .Include(p => p.ProductSizes)
                .Include(p => p.Category)
                //.AsNoTracking()
                .AsQueryable();

            // Filter by category
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

            // Filter by color
            if (!string.IsNullOrEmpty(color))
            {
                query = query.Where(p => p.ProductSizes.Any(ps => ps.Color.ToLower() == color.ToLower()));
            }

            // Filter by size
            if (!string.IsNullOrEmpty(size))
            {
                query = query.Where(p => p.ProductSizes.Any(ps => ps.Size.ToLower() == size.ToLower()));
            }

            // Filter by price
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

            // Filter by collection
            if (!string.IsNullOrEmpty(collection))
            {
                query = query.Where(p => p.Collections != null && p.Collections.ToLower().Contains(collection.ToLower()));
            }

            // Filter by availability
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

            // Filter by gender
            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(p => p.Gender != null && p.Gender.ToLower() == gender.ToLower());
            }

            // Filter by text search
            if (!string.IsNullOrEmpty(text))
            {
                query = query.Where(p => p.ProductName.ToLower().Contains(text.ToLower()));
            }

            // Count products for filters
            availableCount = query.Count(p => p.StockQuantity > 0);
            outOfStockCount = query.Count(p => p.StockQuantity == 0);

            // Pagination
            int totalProducts = query.Count();
            totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
            
            return query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public Models.Product GetProductDetails(int id)
        {
            return _context.Products
                .Include(p => p.ProductSizes)
                .FirstOrDefault(p => p.ProductId == id);
        }

        public bool IsFavourite(int productId, int userId)
        {
            return _context.Favourites.Any(f => f.UserId == userId && f.ProductId == productId);
        }

        public List<string> GetAvailableColors(int productId, string size)
        {
            if (string.IsNullOrEmpty(size))
                return new List<string>();

            return _context.ProductsSize
                .Where(ps => ps.ProductId == productId && ps.Size == size && ps.Quantity > 0)
                .Select(ps => ps.Color)
                .Distinct()
                .ToList();
        }
    }
} 