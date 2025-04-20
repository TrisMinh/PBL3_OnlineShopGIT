using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PBL3_OnlineShop.Controllers
{
    public class FavouriteController : Controller
    {
        // GET: FavouriteController
        public ActionResult Index()
        {
            return View();
        }

        // GET: FavouriteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FavouriteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FavouriteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FavouriteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FavouriteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FavouriteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FavouriteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
