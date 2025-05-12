namespace PBL3_OnlineShop.Models.ViewModels.Statistic
{
    public class AllStatisticView
    {
        public List<TransactionsView> Transactions { get; set; }
        public ChartDataView ChartData { get; set; }
        public DashboardStatisticsView DashboardStatistics { get; set; }

    }
}
