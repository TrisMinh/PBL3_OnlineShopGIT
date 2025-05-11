using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PBL3_OnlineShop.Validation
{
    public class RoleAuthorizationAttribute : ActionFilterAttribute
    {
        private readonly string _role;
        public RoleAuthorizationAttribute(string role)
        {
            _role = role;
        }
        // Phương thức thực thi trước khi action được gọi
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("_Role");

            if (role == null)
            {
                context.Result = new RedirectToActionResult("Login", "Account", new { area = "" });
            }
            else if (role != _role)
            {
                context.Result = new RedirectToActionResult("Index", "Home", new { area = "" });
            }
            // Tiếp tục xử lý action nếu phân quyền hợp lệ
            base.OnActionExecuting(context);
        }
    }
}
