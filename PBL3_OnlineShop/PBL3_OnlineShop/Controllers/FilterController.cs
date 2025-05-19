using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace PBL3_OnlineShop.Controllers
{
    public class FilterController : Controller
    {
        // Action lưu trạng thái mở/đóng của filter
        [HttpGet]
        [HttpPost]
        public IActionResult ToggleFilterState(string filterType)
        {
            // Lấy danh sách filters đang mở từ session
            string openFiltersJson = HttpContext.Session.GetString("OpenFilters");
            List<string> openFilters = string.IsNullOrEmpty(openFiltersJson)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(openFiltersJson);

            // Nếu filter đã mở, thì đóng nó (xóa khỏi danh sách)
            if (openFilters.Contains(filterType))
            {
                openFilters.Remove(filterType);
            }
            // Nếu filter chưa mở, thì mở nó (thêm vào danh sách)
            else
            {
                openFilters.Add(filterType);
            }

            // Lưu lại vào session
            HttpContext.Session.SetString("OpenFilters", JsonSerializer.Serialize(openFilters));

            // Lấy URL hiện tại để chuyển hướng về
            string returnUrl = Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Url.Action("Index", "Products");
            }
            return Redirect(returnUrl);
        }

        // Action áp dụng filter
        [HttpGet]
        public IActionResult Apply(string type, string value)
        {
            // Lấy URL hiện tại
            var returnUrl = HttpContext.Request.Headers["Referer"].ToString();
            var uri = new Uri(returnUrl);
            
            // Tạo danh sách các tham số truy vấn mới
            var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);
            
            // Nếu value rỗng hoặc null, xóa param này
            if (string.IsNullOrEmpty(value))
            {
                queryParams.Remove(type);
            }
            // Ngược lại, set giá trị mới
            else
            {
                queryParams.Set(type, value);
            }
            
            // Reset page về 1
            queryParams.Remove("page");
            
            // Tạo URL mới
            string url = uri.GetLeftPart(UriPartial.Path);
            if (queryParams.Count > 0)
            {
                url += "?" + queryParams.ToString();
            }
            
            return Redirect(url);
        }
    }
} 