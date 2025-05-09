using Microsoft.AspNetCore.Mvc;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Repository;

namespace PBL3_OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CouponController : Controller
    {
        private readonly PBL3_Db_Context _context;
        public CouponController(PBL3_Db_Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var coupons = _context.Coupons.ToList();
            return View(coupons);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(); // Trả về trang Create.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                _context.Coupons.Add(coupon);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Thêm không thành công!";
            }
            return View(coupon);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var coupon = _context.Coupons.FirstOrDefault(p => p.Id == id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                _context.Coupons.Update(coupon);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Cập nhật không thành công!";
            }
            return View(coupon);
        }
    }
}
