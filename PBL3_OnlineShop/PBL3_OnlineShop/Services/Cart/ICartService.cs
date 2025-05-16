using PBL3_OnlineShop.Models.ViewModels;

namespace PBL3_OnlineShop.Services.Cart
{
    public interface ICartService
    {
        public CartItemView GetCartView(int? userId, ISession session);
        public Models.Cart GetCartByIdUser(int userId);
        public Models.ProductSize GetProductSizeByIdSizeColor(int productId, string size, string color);
        public Models.User GetUserById(int userId);
        public Models.Product GetProductByID(int productId);
        public string CheckCartItem(int userId);
        public void AddToCart(int? userId, int productId, string size, string color, ISession session, Models.Product product);
        public void IncreaseQuantity(int? userId, int productId, string size, string color, ISession session);
        public void DecreaseQuantity(int? userId, int productId, string size, string color, ISession session);
        public void RemoveItem(int? userId, int productId, string size, string color, ISession session);
    }
}
