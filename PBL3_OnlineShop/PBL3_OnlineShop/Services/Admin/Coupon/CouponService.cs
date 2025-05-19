using PBL3_OnlineShop.Data;

namespace PBL3_OnlineShop.Services.Admin.Coupon
{
    public class CouponService : ICouponService
    {
        private readonly PBL3_Db_Context _context;
        public CouponService(PBL3_Db_Context context)
        {
            _context = context;
        }
        public List<Models.Coupon> GetAllCoupons()
        {
            return _context.Coupons.OrderByDescending(p => p.Id).ToList();
        }
        public Models.Coupon GetCouponById(int id)
        {
            return _context.Coupons.FirstOrDefault(p => p.Id == id);
        }
        public bool CreateCoupon(Models.Coupon coupon)
        {
            if (_context.Coupons.Any(p => p.Name.ToLower() == coupon.Name.ToLower()))
            {
                return false;
            }
            _context.Coupons.Add(coupon);
            _context.SaveChanges();
            return true;
        }
        public bool UpdateCoupon(Models.Coupon coupon)
        {
            if (_context.Coupons.Any(p => p.Name == coupon.Name && p.Id != coupon.Id))
            {
                return false;
            }
            var existingCoupon = _context.Coupons.FirstOrDefault(p => p.Id == coupon.Id);
            existingCoupon.Name = coupon.Name;
            existingCoupon.Description = coupon.Description;
            existingCoupon.Quantity = coupon.Quantity;
            existingCoupon.StartDate = coupon.StartDate;
            existingCoupon.EndDate = coupon.EndDate;
            existingCoupon.Discount = coupon.Discount;
            _context.Coupons.Update(existingCoupon);
            _context.SaveChanges();
            return true;
        }
        public bool DeleteCoupon(int id)
        {
            var coupon = _context.Coupons.FirstOrDefault(p => p.Id == id);
            if (coupon == null)
            {
                return false;
            }

            var relatedUsages = _context.CouponUsages.Where(cu => cu.CouponId == id).ToList();
            if (relatedUsages.Any()) 
            {
                _context.CouponUsages.RemoveRange(relatedUsages);
                _context.SaveChanges();
            }

            _context.Coupons.Remove(coupon);
            _context.SaveChanges();
            return true;
        }
        public List<Models.Coupon> SearchCoupon(int? couponId, string couponName, decimal? couponDiscount, int status)
        {
            var query = from p in _context.Coupons
                        where (!couponId.HasValue || couponId == p.Id) &&
                        (string.IsNullOrEmpty(couponName) || p.Name.Contains(couponName)) &&
                        (!couponDiscount.HasValue || couponDiscount == p.Discount) &&
                        (status == -1 || status == p.status)
                        select p;
            var coupons = query.ToList();
            return coupons;
        }
    }
}
