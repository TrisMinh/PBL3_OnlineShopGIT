using Microsoft.AspNetCore.Mvc;
using PBL3_OnlineShop.Migrations;
using PBL3_OnlineShop.Repository;

namespace PBL3_OnlineShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly PBL3_Db_Context _context;
        public CheckoutController(PBL3_Db_Context context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            return View();
        }


    }
}
