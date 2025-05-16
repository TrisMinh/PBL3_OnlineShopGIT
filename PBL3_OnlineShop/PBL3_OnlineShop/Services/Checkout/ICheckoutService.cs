using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Models.ViewModels;

namespace PBL3_OnlineShop.Services.Checkout
{
    public interface ICheckoutService
    {
        public CheckoutView GetCheckoutView(int userId, string couponUsed);
        public Models.Coupon GetCouponByName(string couponName);
        public string CheckCoupon(int userId, string couponName);
        public decimal CalculateTotalPrice(int userId, string couponName);
        public string CheckProductSize(List<CartItem> cartItems);
        public Models.Cart GetCartByUserId(int userId);
        public List<CartItem> GetListCartItemsByCartId(int cartId);
        public void CreateOrderInDatabase(decimal TotalPrice, string CouponUsed, List<CartItem> cartItems, int userId, List<int> productIds, string randomCode = null);
        public List<int> GetListProductIdFromCartItems(List<CartItem> cartItems, int cartId);
        public Models.User GetUserById(int userId);
    }
}
