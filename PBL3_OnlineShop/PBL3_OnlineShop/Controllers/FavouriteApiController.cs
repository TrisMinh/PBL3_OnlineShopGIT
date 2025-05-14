using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PBL3_OnlineShop.Services.Favourite;

namespace PBL3_OnlineShop.Controllers
{
    [Route("api/favourite")]
    [ApiController]
    public class FavouriteApiController : ControllerBase
    {
        private readonly IFavouriteService _favouriteService;
        
        public FavouriteApiController(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }

        [HttpPost("toggle/{productId}")]
        public IActionResult Toggle(int productId)
        {
            int? userId = HttpContext.Session.GetInt32("_UserId");
            if (userId == null)
            {
                return Unauthorized(new { success = false, message = "Bạn cần đăng nhập để sử dụng chức năng này." });
            }
            
            bool isFavourite = _favouriteService.ToggleFavourite(userId.Value, productId);
            return Ok(new { success = true, isFavourite = isFavourite });
        }
    }
} 