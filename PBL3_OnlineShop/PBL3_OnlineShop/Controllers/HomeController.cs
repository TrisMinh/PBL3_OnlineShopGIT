using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Data;
using System.Diagnostics;
using PBL3_OnlineShop.Services.Home;

namespace PBL3_OnlineShop.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public IActionResult Index()
        {
            var hotProducts = _homeService.GetHotProducts();

            ViewBag.HotProducts = hotProducts;

            var saleProducts = _homeService.GetSaleProducts();

            ViewBag.SaleProducts = saleProducts;

            var allProducts = _homeService.GetAllProducts();

            ViewBag.AllProducts = allProducts;

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
