namespace PBL3_OnlineShop.Models.ViewModels.Statistic
{
    public class DashboardStatisticsView
    {
        public int Id { get; set; }
        public int UserCount { get; set; }
        public decimal IncreaseUserPercentage { get; set; }
        public decimal TotalSale { get; set; }
        public decimal IncreaseSalePercentage { get; set; }
        public int OrderCount { get; set; }
        public decimal IncreaseOrderPercentage { get; set; }
        public int OrderStatusCount { get; set; }
        public decimal IncreasePendingOrderPercentage { get; set; }

    }
}
