using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Data;

namespace PBL3_OnlineShop.Services.Inventory
{
    public class InventoryService: IInventoryService
    {
        private readonly PBL3_Db_Context _context;
        public InventoryService(PBL3_Db_Context context)
        {
            _context = context;
        }
        public void UpdateProductsStockQuantity(List<int> productIds)
        {
            foreach (var productId in productIds)
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
                if (product != null)
                {
                    // Tính tổng số lượng từ ProductsSize
                    var totalQuantity = _context.ProductsSize
                        .Where(ps => ps.ProductId == productId)
                        .Sum(ps => ps.Quantity);

                    // Cập nhật StockQuantity
                    product.StockQuantity = totalQuantity;
                    _context.Products.Update(product);
                }
            }
            _context.SaveChanges();
        }
    }
}
