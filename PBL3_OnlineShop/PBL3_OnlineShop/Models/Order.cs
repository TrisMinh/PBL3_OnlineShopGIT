namespace PBL3_OnlineShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime OrderDate { get; set; }
        public int Status { get; set; }
        public decimal TotalPrice { get; set; }
        public string CouponUsed { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
