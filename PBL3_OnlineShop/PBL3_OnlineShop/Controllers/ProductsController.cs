using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Models;
using System.Collections.Generic; // Required for List
using System.Linq; // Required for FirstOrDefault, Contains

namespace PBL3_OnlineShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly PBL3_Db_Context _context;

        public ProductsController(PBL3_Db_Context context)
        {
            _context = context;
        }
        // GET: ProductsController
        public ActionResult Index(string category)
        {
            var categories = new List<string> { "New", "Shirts", "Polo Shirts", "Shorts", "Suits", "Best sellers", "T-Shirts", "Jeans", "Jackets", "Coats" };

            var products = new List<Product>();

            string selectedCategory = null;

            if (!string.IsNullOrEmpty(category) && categories.Contains(category))
            {
                selectedCategory = category;
            }
            else
            {
                selectedCategory = categories.FirstOrDefault();
            }

            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = selectedCategory;

            return View(products);
        }

        // GET: ProductController/Details/5
        public  ActionResult Details(int id = 1)
        {
            if (id == 0) return View();

            var productbyId = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (productbyId == null)
            {
                return RedirectToAction("Index");
            }

            return View(productbyId);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

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

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // Original code
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // Original code
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
