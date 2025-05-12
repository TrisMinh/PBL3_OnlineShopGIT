using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PBL3_OnlineShop.Data;
using System.Linq;

namespace PBL3_OnlineShop.Controllers
{
    [Route("api/favourite")]
    [ApiController]
    public class FavouriteApiController : ControllerBase
    {
        private readonly PBL3_Db_Context _db;
        public FavouriteApiController(PBL3_Db_Context db)
        {
            _db = db;
        }

        [HttpPost("toggle/{productId}")]
        public IActionResult Toggle(int productId)
        {
            int? userId = HttpContext.Session.GetInt32("_UserId");
            if (userId == null)
            {
                return Unauthorized(new { success = false, message = "Bạn cần đăng nhập để sử dụng chức năng này." });
            }
            var fav = _db.Favourites.FirstOrDefault(f => f.UserId == userId && f.ProductId == productId);
            if (fav != null)
            {
                _db.Favourites.Remove(fav);
                _db.SaveChanges();
                return Ok(new { success = true, isFavourite = false });
            }
            else
            {
                _db.Favourites.Add(new Models.Favourite { UserId = userId.Value, ProductId = productId });
                _db.SaveChanges();
                return Ok(new { success = true, isFavourite = true });
            }
        }
    }
} 