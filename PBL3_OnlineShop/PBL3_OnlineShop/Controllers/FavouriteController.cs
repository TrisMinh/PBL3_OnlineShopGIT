using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PBL3_OnlineShop.Services.Favourite;

namespace PBL3_OnlineShop.Controllers
{
    public class FavouriteController : Controller
    {
        private readonly IFavouriteService _favouriteService;

        public FavouriteController(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }

        // GET: FavouriteController
        public ActionResult Index(int page = 1)
        {
            int? userId = HttpContext.Session.GetInt32("_UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int pageSize = 40;
            var favourites = _favouriteService.GetFavouritesByUserId(userId.Value, page, pageSize);
            int totalFavourites = _favouriteService.GetTotalFavourites(userId.Value);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalFavourites / pageSize);

            return View(favourites);
        }

        // GET: FavouriteController/Create
        public ActionResult Create(int id)
        {
            int? userId = HttpContext.Session.GetInt32("_UserId");
            Console.WriteLine("Vào action Favourite/Create với id = " + id + ", userId = " + userId);

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return RedirectToAction("Details", "Products", new { id = id });
        }

        // GET: FavouriteController/Delete/5
        public ActionResult Delete(int id)
        {
            int? userId = HttpContext.Session.GetInt32("_UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            bool result = _favouriteService.RemoveFavourite(id, userId.Value);

            return RedirectToAction(nameof(Index));
        }
    }
}
