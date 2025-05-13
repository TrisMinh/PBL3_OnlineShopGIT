using Microsoft.AspNetCore.Mvc;
using PBL3_OnlineShop.Validation;
using PBL3_OnlineShop.Models.ViewModels.Statistic;
using PBL3_OnlineShop.Services.Admin.Statistic;

namespace PBL3_OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RoleAuthorization("Admin")]
    public class StatisticController : Controller
    {
        private readonly IStatisticService _service;
        public StatisticController(IStatisticService service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var viewmodel = new AllStatisticView()
            {
                Transactions = _service.GetRecentTransactions(5),
                ChartData = _service.GetMonthlyRevenueData(),
                DashboardStatistics = _service.GetDashboardStatistics()
            };
            return View(viewmodel);
        }
        [HttpGet]
        public IActionResult GetRevenueData(string type)
        {
            ChartDataView result = type switch
            {
                "yearly" => _service.GetYearlyRevenueData(),
                "monthly" => _service.GetMonthlyRevenueData(),
                "daily" => _service.GetDailyRevenueData(),
                _ => _service.GetMonthlyRevenueData() // default
            };

            return Json(result);
        }


    }
}
