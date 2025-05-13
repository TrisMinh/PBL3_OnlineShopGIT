using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Validation;
using PBL3_OnlineShop.Services.Admin.User;

namespace PBL3_OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RoleAuthorization("Admin")]
    public class UserController : Controller
    {      
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public ActionResult Index()
        {
            return View(_userService.GetAllUsers());
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.RoleList = new List<SelectListItem>
            {
                new SelectListItem { Text = "Customer", Value = "Customer" },
                new SelectListItem { Text = "Admin", Value = "Admin" }
            };
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            ViewBag.RoleList = new List<SelectListItem>
            {
                new SelectListItem { Text = "Customer", Value = "Customer" },
                new SelectListItem { Text = "Admin", Value = "Admin" }
            };
            var result = _userService.CreateUser(user);
            if (!result)
            {
                TempData["Error"] = "Thêm không thành công";
                return View(user);
            }
            TempData["Success"] = "Thêm thành công!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = _userService.GetUserById(id);
            ViewBag.RoleList = new List<SelectListItem>
            {
                new SelectListItem { Text = "Customer", Value = "Customer" },
                new SelectListItem { Text = "Admin", Value = "Admin" }
            };
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, User user)
        {
            try
            {
                ViewBag.RoleList = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Customer", Value = "Customer" },
                    new SelectListItem { Text = "Admin", Value = "Admin" }
                };

                var result = _userService.UpdateUser(id, user);
                if (!result)
                {
                    TempData["Error"] = "Không tìm thấy user.";
                    return NotFound();
                }

                TempData["Success"] = "Cập nhật user thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi: " + ex.Message;
                return View(user);
            }
        }

        public ActionResult Delete(int id)
        {
            var result = _userService.DeleteUser(id);
            if (!result)
            {
                TempData["Error"] = "Không tìm thấy user.";
                return NotFound();
            }
            TempData["Success"] = "Xóa user thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
