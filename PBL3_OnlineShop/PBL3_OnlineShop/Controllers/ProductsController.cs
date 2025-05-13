using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PBL3_OnlineShop.Services.Product;
using System;
using System.Collections.Generic;

namespace PBL3_OnlineShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: ProductsController
        public ActionResult Index(string category, string color, string size, string price, string collection, string availability, string gender, string text, int page = 1)
        {
            var categories = new List<string> { "New", "Sales", "Polo Shirts", "Shorts", "Suits", "Best sellers", "T-Shirts", "Jeans", "Jackets", "Coats" };

            int pageSize = 30;
            int totalPages;
            int availableCount;
            int outOfStockCount;

            var products = _productService.GetProducts(
                category, color, size, price, collection, availability, gender, text, 
                page, pageSize, out totalPages, out availableCount, out outOfStockCount);

            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = category;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SelectedCollection = collection;
            ViewBag.SelectedText = text;
            ViewBag.AvailableCount = availableCount;
            ViewBag.OutOfStockCount = outOfStockCount;

            int? userId = HttpContext.Session.GetInt32("_UserId");
            if (userId == null)
            {
                Console.WriteLine("Chưa đăng nhập hoặc session không có UserId");
            }
            else
            {
                Console.WriteLine("UserId trong session: " + userId);
            }

            return View(products);
        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            var product = _productService.GetProductDetails(id);

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            // Lấy userId từ session
            int? userId = HttpContext.Session.GetInt32("_UserId");
            bool isFavourite = false;
            if (userId != null)
            {
                isFavourite = _productService.IsFavourite(id, userId.Value);
            }
            ViewBag.IsFavourite = isFavourite;

            return View(product);
        }

        [HttpGet]
        public IActionResult GetAvailableColors(int productId, string size)
        {
            var colors = _productService.GetAvailableColors(productId, size);
            return Json(colors);
        }
    }
}
