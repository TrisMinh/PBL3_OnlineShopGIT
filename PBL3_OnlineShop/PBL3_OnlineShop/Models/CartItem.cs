namespace PBL3_OnlineShop.Models
{
    public class CartItem
    {
        public CartItem() 
        {
            
        }
        public CartItem(Product product)
        {
            ProductId = product.ProductId;
            ProductName = product.ProductName;
            Quantity = 1;
            SellingPrice = product.SellingPrice;
            ImageUrl = product.ImageUrl;
        }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal TotalPrice 
        {
            get { return SellingPrice * Quantity; }
        }
        public string ImageUrl { get; set; }
    }
}
