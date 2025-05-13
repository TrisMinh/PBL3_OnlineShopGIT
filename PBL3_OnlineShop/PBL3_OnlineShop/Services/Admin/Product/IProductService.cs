using PBL3_OnlineShop.Models;

namespace PBL3_OnlineShop.Services.Admin.Product
{
    public interface IProductService
    {
        public List<Models.Product> GetAllProducts();
        public Models.Product GetProductById(int id);
        public bool CreateProduct(Models.Product product, List<ProductSize> sizes);
        public bool UpdateProduct(int id, Models.Product product, List<ProductSize> sizes);
        public bool DeleteProduct(int id);
        public List<Models.Product> SearchProducts(int? productID, string productName, decimal? price, string category, string status, string gender);
        public void SyncStockQuantities();
        public List<Models.Category> GetCategorySelectList();
    }
}
