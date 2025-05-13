using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Validation;
using System.Linq;
using PBL3_OnlineShop.Services.Admin.Product;

namespace PBL3_OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RoleAuthorization("Admin")]    
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        public ActionResult Index()
        {   
            return View(_productService.GetAllProducts());
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(_productService.GetCategorySelectList(), "CategoryId", "CategoryName"); // Lấy danh sách Category từ DbContext
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, List<ProductSize> Sizes)
        {
            ViewBag.Categories = new SelectList(_productService.GetCategorySelectList(), "CategoryId", "CategoryName", product.CategoryId);

            var result = _productService.CreateProduct(product, Sizes);
            if(!result)
            {
                TempData["Error"] = "Thêm sản phẩm không thành công.";
                return View(product);
            }
            TempData["Success"] = "Thêm sản phẩm thành công!";
            return View(product);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            ViewBag.Categories = new SelectList(_productService.GetCategorySelectList(),"CategoryId", "CategoryName", product.CategoryId);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product product, List<ProductSize> Sizes)
        {
            ViewBag.Categories = new SelectList(_productService.GetCategorySelectList(), "CategoryId", "CategoryName", product.CategoryId);
            var result = _productService.UpdateProduct(id, product, Sizes);
            if (!result)
            {
                TempData["Error"] = "Cập nhật sản phẩm không thành công.";
                return View(product);
            }
            TempData["Success"] = "Cập nhật sản phẩm thành công!";
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var result = _productService.DeleteProduct(id);
            if (!result)
            {
                TempData["Error"] = "Xoá sản phẩm không thành công.";
            }
            TempData["Success"] = "Xóa sản phẩm thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Search(int? productID, string productName, decimal? price, string category, string status, string gender)
        {
            ViewBag.productID = productID;
            ViewBag.productName = productName;
            ViewBag.price = price;
            ViewBag.category = category;
            ViewBag.status = status;
            ViewBag.gender = gender;

            return View("Index", _productService.SearchProducts(productID, productName, price, category, status, gender));
        }
    }
}
