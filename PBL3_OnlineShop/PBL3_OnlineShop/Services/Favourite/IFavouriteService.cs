using PBL3_OnlineShop.Models;

namespace PBL3_OnlineShop.Services.Favourite
{
    public interface IFavouriteService
    {
        public List<Models.Favourite> GetFavouritesByUserId(int userId, int page = 1, int pageSize = 40);
        public int GetTotalFavourites(int userId);
        public Models.Favourite GetFavourite(int userId, int productId);
        public bool AddFavourite(int userId, int productId);
        public bool RemoveFavourite(int favouriteId, int userId);
        public bool ToggleFavourite(int userId, int productId);
    }
} 