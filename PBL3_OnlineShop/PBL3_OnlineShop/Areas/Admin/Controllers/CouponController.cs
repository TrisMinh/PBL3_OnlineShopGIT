using Microsoft.AspNetCore.Mvc;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Validation;
using PBL3_OnlineShop.Services.Admin.Coupon;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PBL3_OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RoleAuthorization("Admin")]
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        public ActionResult Index()
        {
            return View(_couponService.GetAllCoupons());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Coupon coupon)
        {
            if (coupon.StartDate > coupon.EndDate)
            {
                TempData["Error"] = "Ngày bắt đầu không được lớn hơn ngày kết thúc";
                return View(coupon);
            }
            var result = _couponService.CreateCoupon(coupon);
            if (!result)
            {
                TempData["Error"] = "Mã giảm giá đã tồn tại!";
                return View(coupon);
            }
            TempData["Success"] = "Thêm mã giảm giá thành công!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var coupon = _couponService.GetCouponById(id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Coupon coupon)
        {
            if (coupon.StartDate > coupon.EndDate)
            {
                TempData["Error"] = "Ngày bắt đầu không được lớn hơn ngày kết thúc";
                return View(coupon);
            }
            var result = _couponService.UpdateCoupon(coupon);
            if (!result)
            {
                TempData["Error"] = "Mã giảm giá đã tồn tại!";
                return View(coupon);
            }
            TempData["Success"] = "Cập nhật mã giảm giá thành công!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Search(int? couponId, string couponName, decimal? couponDiscount, int status)
        {
            ViewBag.couponID = couponId;
            ViewBag.couponName = couponName;
            ViewBag.couponDiscount = couponDiscount;
            ViewBag.status = status;

            return View("Index", _couponService.SearchCoupon(couponId, couponName, couponDiscount, status));
        }

        public ActionResult Delete(int id)
        {
            var result = _couponService.DeleteCoupon(id);
            if (!result)
            {
                TempData["Error"] = "Mã giảm giá không tồn tại!";
                return RedirectToAction("Index");
            }
            TempData["Success"] = "Xóa mã giảm giá thành công!";
            return RedirectToAction("Index");
        }
    }
}
