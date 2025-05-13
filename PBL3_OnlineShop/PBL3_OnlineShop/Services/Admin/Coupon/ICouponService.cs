namespace PBL3_OnlineShop.Services.Admin.Coupon
{
    public interface ICouponService
    {
        public List<Models.Coupon> GetAllCoupons();
        public Models.Coupon GetCouponById(int id);
        public bool CreateCoupon(Models.Coupon coupon);
        public bool UpdateCoupon(Models.Coupon coupon);
        public bool DeleteCoupon(int id);
        public List<Models.Coupon> SearchCoupon(int? couponId, string couponName, decimal? couponDiscount, int status);

    }
}
