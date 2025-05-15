using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Models.ViewModels.Statistic;
using PBL3_OnlineShop.Data;

namespace PBL3_OnlineShop.Services.Admin.Statistic
{
    public class StatisticService : IStatisticService
    {
        private readonly PBL3_Db_Context _context;
        public StatisticService(PBL3_Db_Context context)
        {
            _context = context;
        }

        public decimal CalculatePercentageChange(decimal currentValue, decimal previousValue)
        {
            if (previousValue == 0)
            {
                return currentValue == 0 ? 0 : 100;
            }
            return Math.Round((decimal) ((currentValue - previousValue) / previousValue * 100), 2);
        }

        public DashboardStatisticsView GetDashboardStatistics()
        {
            var now = DateTime.Now;

            var increlastmonth = _context.Users.Count(u => u.Role == "Customer" && u.CreatedAt.Month == now.AddMonths(-1).Month);
            var nowmonthUser = _context.Users.Count(u => u.Role == "Customer" && u.CreatedAt.Month == now.Month);

            var increlastmonthSale = _context.Orders.Where(o => o.Status == 2 && o.OrderDate.Month == now.AddMonths(-1).Month).Sum(o => o.TotalPrice);
            var nowmonthSale = _context.Orders.Where(o => o.Status == 2 && o.OrderDate.Month == now.Month).Sum(o => o.TotalPrice);

            var increlastmonthOrder = _context.Orders.Count(o => o.Status == 2 && o.OrderDate.Month == now.AddMonths(-1).Month);
            var nowmonthOrder = _context.Orders.Count(o => o.Status == 2 && o.OrderDate.Month == now.Month);

            var increlastmonthPendingOrder = _context.Orders.Count(o => o.Status == 1 && o.OrderDate.Month == now.AddMonths(-1).Month);
            var nowmonthPendingOrder = _context.Orders.Count(o => o.Status == 1 && o.OrderDate.Month == now.Month);

            return new DashboardStatisticsView
            {
                UserCount = _context.Users.Count(u => u.Role == "Customer"),
                IncreaseUserPercentage = CalculatePercentageChange(nowmonthUser, increlastmonth),

                TotalSale = _context.Orders.Where(o => o.Status == 2).Sum(o => o.TotalPrice),
                IncreaseSalePercentage = CalculatePercentageChange(nowmonthSale, increlastmonthSale),

                OrderCount = _context.Orders.Count(),
                IncreaseOrderPercentage = CalculatePercentageChange(nowmonthOrder, increlastmonthOrder),

                OrderStatusCount = _context.Orders.Count(o => o.Status == 1),
                IncreasePendingOrderPercentage = CalculatePercentageChange(nowmonthPendingOrder, increlastmonthPendingOrder),
            };
        }
        public List<TransactionsView> GetRecentTransactions(int count = 5)
        {
            return _context.Orders.Include(o => o.User).OrderByDescending(o => o.OrderDate).Take(count)
                .Select(o => new TransactionsView
                {
                    OrderId = o.Id,
                    CustomerName = o.User.UserName,
                    OrderDate = o.OrderDate,
                    Amount = o.TotalPrice,
                    Status = o.Status
                }).ToList();
        }
        public ChartDataView GetYearlyRevenueData()
        {
            // Lấy doanh thu theo năm từ cơ sở dữ liệu
            var data = _context.Orders.Where(o => o.Status == 2) // Chỉ tính đơn hàng đã hoàn thành
                .GroupBy(o => o.OrderDate.Year) // Nhóm theo năm
                .Select(g => new
                {
                    Year = g.Key,
                    Total = g.Sum(o => o.TotalPrice)
                }).OrderBy(x => x.Year)
                .ToList();

            // Lấy các năm từ năm hiện tại trừ 19 năm về trước
            var allYears = Enumerable.Range(DateTime.Now.Year - 19, 20).ToList();

            var yearsData = allYears.Select(year =>
            {
                var yearData = data.FirstOrDefault(d => d.Year == year);
                return new
                {
                    Year = $"{year}",
                    Total = yearData?.Total ?? 0 // Nếu không có dữ liệu, trả về 0
                };
            }).ToList();

            return new ChartDataView
            {
                Labels = yearsData.Select(d => d.Year).ToList(),
                Values = yearsData.Select(d => d.Total).ToList()
            };
        }
        public ChartDataView GetMonthlyRevenueData()
        {
            var data = _context.Orders
                .Where(o => o.Status == 2) // Chỉ tính những đơn hàng đã hoàn tất (Status == 2)
                .GroupBy(o => o.OrderDate.Month) // Nhóm theo tháng
                .Select(g => new
                {
                    Month = g.Key,
                    Total = g.Sum(o => o.TotalPrice)
                })
                .OrderBy(x => x.Month) // Đảm bảo thứ tự từ tháng 1 đến tháng 12
                .ToList();

            // Đảm bảo trả về dữ liệu cho tất cả 12 tháng, kể cả tháng không có dữ liệu
            var allMonths = Enumerable.Range(1, 12).ToList();
            var monthsData = allMonths.Select(month =>
            {
                var monthData = data.FirstOrDefault(d => d.Month == month);
                return new
                {
                    Month = $"Tháng {month}",
                    Total = monthData?.Total ?? 0 // Nếu không có dữ liệu, trả về 0
                };
            }).ToList();

            return new ChartDataView
            {
                Labels = monthsData.Select(d => d.Month).ToList(),
                Values = monthsData.Select(d => d.Total).ToList()
            };
        }
        public ChartDataView GetDailyRevenueData()
        {
            // Xác định ngày bắt đầu và ngày kết thúc của tháng hiện tại
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            // Lấy doanh thu theo ngày trong tháng hiện tại
            var data = _context.Orders.Where(o => o.Status == 2 && o.OrderDate >= firstDayOfMonth && o.OrderDate <= lastDayOfMonth)
                .GroupBy(o => o.OrderDate.Date) // Nhóm theo ngày
                .Select(g => new
                {
                    Date = g.Key,
                    Total = g.Sum(o => o.TotalPrice)
                })
                .OrderBy(x => x.Date)
                .ToList();

            // Tạo danh sách tất cả các ngày trong tháng
            var allDaysInMonth = Enumerable.Range(0, (lastDayOfMonth - firstDayOfMonth).Days + 1)
                .Select(i => firstDayOfMonth.AddDays(i))
                .ToList();

            // Đảm bảo trả về doanh thu cho tất cả các ngày trong tháng, kể cả ngày không có dữ liệu
            var daysData = allDaysInMonth.Select(day =>
            {
                var dayData = data.FirstOrDefault(d => d.Date == day.Date);
                return new
                {
                    Date = day.ToString("dd"),
                    Total = dayData?.Total ?? 0 // Nếu không có dữ liệu, trả về 0
                };
            }).ToList();

            return new ChartDataView
            {
                Labels = daysData.Select(d => d.Date).ToList(),
                Values = daysData.Select(d => d.Total).ToList()
            };
        }
    }
}
