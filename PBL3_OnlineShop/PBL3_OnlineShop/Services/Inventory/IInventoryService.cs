namespace PBL3_OnlineShop.Services.Inventory
{
    public interface IInventoryService
    {
        public void UpdateProductsStockQuantity(List<int> productIds);
    }
}
