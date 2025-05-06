using Microsoft.AspNetCore.Mvc;
using PBL3_OnlineShop.Repository;

namespace PBL3_OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly PBL3_Db_Context _context;

        public ProductsController(PBL3_Db_Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
