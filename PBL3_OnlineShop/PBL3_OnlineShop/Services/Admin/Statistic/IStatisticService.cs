using PBL3_OnlineShop.Models.ViewModels.Statistic;

namespace PBL3_OnlineShop.Services.Admin.Statistic
{
    public interface IStatisticService
    {
        public DashboardStatisticsView GetDashboardStatistics();
        public List<TransactionsView> GetRecentTransactions(int count = 5);
        public ChartDataView GetYearlyRevenueData();
        public ChartDataView GetMonthlyRevenueData();
        public ChartDataView GetDailyRevenueData();
        public decimal CalculatePercentageChange(decimal currentValue, decimal previousValue);
    }
}
