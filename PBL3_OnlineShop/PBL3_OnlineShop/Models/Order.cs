namespace PBL3_OnlineShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserName { get; set; }
        public DateTime OrderDate { get; set; }
        public int Status { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
