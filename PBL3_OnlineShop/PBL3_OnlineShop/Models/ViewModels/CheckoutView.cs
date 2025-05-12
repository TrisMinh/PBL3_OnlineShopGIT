namespace PBL3_OnlineShop.Models.ViewModels
{
    public class CheckoutView
    {
        public List<CartItem> CartItems { get; set; } 
        public decimal Subtotal { get; set; } 
        public decimal ShippingCost { get; set; } = 50; 
        public decimal Discount { get; set; } 
        public decimal TotalPrice { get; set; } 
        public string CouponUsed { get; set; }
    }
}
