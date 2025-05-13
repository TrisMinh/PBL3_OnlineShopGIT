using Microsoft.AspNetCore.Mvc;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Models.ViewModels;

namespace PBL3_OnlineShop.Services.Checkout
{
    public class CheckoutService: ICheckoutService
    {
        private readonly PBL3_Db_Context _context;
        public CheckoutService(PBL3_Db_Context context)
        {
            _context = context;
        }
        public CheckoutView GetCheckoutView(int? userId, string couponUsed)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            var cartItems = _context.CartItems.Where(ci => ci.CartId == cart.CartId).ToList();
            decimal subtotal = cartItems.Sum(ci => ci.Quantity * ci.SellingPrice);
            decimal shippingCost = 50;
            decimal discount = 0;
            if (!string.IsNullOrEmpty(couponUsed))
            {
                var coupon = _context.Coupons.FirstOrDefault(c => c.Name == couponUsed);
                discount = coupon.Discount;
            }

            decimal totalPrice = subtotal + shippingCost - discount;

            return new CheckoutView
            {
                CartItems = cartItems,
                Subtotal = subtotal,
                ShippingCost = shippingCost,
                Discount = discount,
                TotalPrice = totalPrice,
                CouponUsed = couponUsed
            };
        }

        public Models.Coupon GetCouponByName(string couponName)
        {
            return _context.Coupons.FirstOrDefault(c => c.Name.ToLower() == couponName.ToLower());
        }
        public string CheckCoupon(int? userId, string couponName)
        {
            var coupon = _context.Coupons.FirstOrDefault(c => c.Name.ToLower() == couponName.ToLower());
            if (coupon == null || coupon.status == 0)
            {
                return "Coupon not found.";
            }
            if (coupon.Quantity <= 0 || coupon.EndDate < DateTime.Now)
            {
                return "Coupon has expired.";
            }
            var couponUsage = _context.CouponUsages.Any(c => c.CouponId == coupon.Id && c.UserId == userId);
            if (couponUsage)
            {
                return "You have already used this coupon.";
            }
            return "OK";
        }
    }
}
