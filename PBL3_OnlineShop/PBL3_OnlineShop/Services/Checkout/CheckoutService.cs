using Microsoft.AspNetCore.Mvc;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Models.ViewModels;

namespace PBL3_OnlineShop.Services.Checkout
{
    public class CheckoutService : ICheckoutService
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

        public decimal CaculateTotalPrice(int? userId, string couponName)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            var cartItems = _context.CartItems.Where(ci => ci.CartId == cart.CartId).ToList();

            decimal subtotal = cartItems.Sum(ci => ci.Quantity * ci.SellingPrice);
            decimal shippingCost = 50;
            decimal discount = 0;
            if (!string.IsNullOrEmpty(couponName))
            {
                var coupon = _context.Coupons.FirstOrDefault(c => c.Name.ToLower() == couponName.ToLower());
                discount = coupon.Discount;
            }

            decimal totalPrice = subtotal + shippingCost - discount;
            return totalPrice;
        }

        public string CheckProductSize(List<CartItem> cartItems)
        {
            foreach (var item in cartItems)
            {
                var productSize = _context.ProductsSize.FirstOrDefault(ps => ps.ProductId == item.ProductId && ps.Color == item.Color && ps.Size == item.Size);
                if (productSize != null)
                {
                    if (productSize.Quantity < item.Quantity)
                    {
                        return "Not enough stock for " + item.ProductName + " Color: " + item.Color + " Size: " + item.Size;
                    }
                }
                else
                {
                    return "Product not found in stock." + item.ProductId + " " + item.ProductName + " Color: " + item.Color + " Size: " + item.Size;
                }
            }
            return "OK";
        }

        public Models.Cart GetCartByUserId(int? userId)
        {
            return _context.Carts.FirstOrDefault(c => c.UserId == userId);
        }
        public List<CartItem> GetListCartItemsByCartId(int cartId)
        {
            return _context.CartItems.Where(c => c.CartId == cartId).ToList();
        }

        public void CreateOrderInDatabase(decimal TotalPrice, string CouponUsed, List<CartItem> cartItems, int userId, List<int> productIds, string randomCode = null)
        {
            foreach (var item in cartItems)
            {
                var productSize = _context.ProductsSize.FirstOrDefault(ps => ps.ProductId == item.ProductId && ps.Color == item.Color && ps.Size == item.Size);
                if (productSize != null)
                {
                    productSize.Quantity -= item.Quantity;
                    _context.ProductsSize.Update(productSize);
                }
            }

            var order = new Models.Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Status = 1,
                TotalPrice = TotalPrice,
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.SellingPrice,
                    Size = item.Size,
                    Color = item.Color,
                    Code = randomCode ?? "0" // Nếu là COD, lưu "0", ngược lại lưu mã ngẫu nhiên
                };
                _context.OrderDetails.Add(orderDetail);
            }

            _context.SaveChanges();

            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart != null)
            {
                foreach (var item in cartItems)
                {
                    var cartItemToRemove = _context.CartItems.FirstOrDefault(c =>
                        c.CartId == cart.CartId &&
                        c.ProductId == item.ProductId &&
                        c.Size == item.Size &&
                        c.Color == item.Color);

                    if (cartItemToRemove != null)
                    {
                        _context.CartItems.Remove(cartItemToRemove);
                    }
                }
                _context.SaveChanges();
            }

            UpdateProductsStockQuantity(productIds);

            if (!string.IsNullOrEmpty(CouponUsed))
            {
                var coupon = _context.Coupons.FirstOrDefault(c => c.Name == CouponUsed);
                if (coupon != null)
                {
                    var couponUsage = new CouponUsage
                    {
                        UserId = userId,
                        CouponId = coupon.Id
                    };

                    _context.CouponUsages.Add(couponUsage);
                    coupon.Quantity -= 1;
                    _context.Coupons.Update(coupon);
                    _context.SaveChanges();
                }
            }
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

        public List<int> GetListProductIdFromCartItems(List<CartItem> cartItems, int cartId)
        {
            return _context.CartItems.Where(c => c.CartId == cartId).Select(c => c.ProductId).ToList();
        }

        public Models.User GetUserById(int? userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }
    }
}
