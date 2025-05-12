using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Data;
using Microsoft.EntityFrameworkCore;

namespace PBL3_OnlineShop.Controllers
{
    public class FavouriteController : Controller
    {
        private readonly PBL3_Db_Context _db;

        public FavouriteController(PBL3_Db_Context db)
        {
            _db = db;
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
            var favouritesQuery = _db.Favourites
                .Include(f => f.Product)
                .ThenInclude(p => p.ProductSizes)
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.Id);

            int totalFavourites = favouritesQuery.Count();
            var favourites = favouritesQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalFavourites / pageSize);

            return View(favourites);
        }

        // GET: FavouriteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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

            // Kiểm tra đã có trong favourite chưa
            var existingFavourite = _db.Favourites
                .FirstOrDefault(f => f.UserId == userId && f.ProductId == id);

            if (existingFavourite != null)
            {
                TempData["notification"] = "Sản phẩm đã có trong danh sách yêu thích!";
                TempData["notificationClass"] = "alert-info";
                return RedirectToAction("Details", "Products", new { id = id });
            }

            var favourite = new Favourite
            {
                UserId = userId.Value,
                ProductId = id
            };

            try
            {
                _db.Favourites.Add(favourite);
                _db.SaveChanges();
                TempData["notification"] = "Đã thêm sản phẩm vào danh sách yêu thích!";
                TempData["notificationClass"] = "alert-success";
            }
            catch (Exception ex)
            {
                TempData["notification"] = "Lỗi khi thêm vào danh sách yêu thích: " + ex.Message;
                TempData["notificationClass"] = "alert-danger";
            }

            return RedirectToAction("Details", "Products", new { id = id });
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
            int? userId = HttpContext.Session.GetInt32("_UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var favourite = _db.Favourites
                .FirstOrDefault(f => f.Id == id && f.UserId == userId);

            if (favourite == null)
            {
                return NotFound();
            }

            _db.Favourites.Remove(favourite);
            _db.SaveChanges();

            TempData["notification"] = "Đã xóa sản phẩm khỏi danh sách yêu thích!";
            TempData["notificationClass"] = "alert-success";
            return RedirectToAction(nameof(Index));
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
