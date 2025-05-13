using PBL3_OnlineShop.Models.ViewModels;

namespace PBL3_OnlineShop.Services.Checkout
{
    public interface ICheckoutService
    {
        public CheckoutView GetCheckoutView(int? userId, string couponUsed);
        public Models.Coupon GetCouponByName(string couponName);
        public string CheckCoupon(int? userId, string couponName);
    }
}
