namespace PBL3_OnlineShop.Models.ViewModels.Statistic
{
    public class TransactionsView
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Amount { get; set; }
        public int Status { get; set; }  // 0: Cancelled, 1: Pending, 2: Completed
    }
}
