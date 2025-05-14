using PBL3_OnlineShop.Models;

namespace PBL3_OnlineShop.Services.Product
{
    public interface IProductService
    {
        List<Models.Product> GetProducts(string category, string color, string size, string price, 
            string collection, string availability, string gender, string text, int page, int pageSize, 
            out int totalPages, out int availableCount, out int outOfStockCount);
        
        Models.Product GetProductDetails(int id);
        
        bool IsFavourite(int productId, int userId);
        
        List<string> GetAvailableColors(int productId, string size);
    }
} 