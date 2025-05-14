namespace PBL3_OnlineShop.Services.Home
{
    public interface IHomeService
    {
        public List<Models.Product> GetHotProducts();
        public List<Models.Product> GetSaleProducts();
        public List<Models.Product> GetAllProducts();

    }
}
