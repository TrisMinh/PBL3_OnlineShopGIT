using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Models;

namespace PBL3_OnlineShop.Services.Favourite
{
    public class FavouriteService : IFavouriteService
    {
        private readonly PBL3_Db_Context _context;
        
        public FavouriteService(PBL3_Db_Context context)
        {
            _context = context;
        }

        public List<Models.Favourite> GetFavouritesByUserId(int userId, int page = 1, int pageSize = 40)
        {
            return _context.Favourites
                .Include(f => f.Product)
                .ThenInclude(p => p.ProductSizes)
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetTotalFavourites(int userId)
        {
            return _context.Favourites
                .Where(f => f.UserId == userId)
                .Count();
        }

        public Models.Favourite GetFavourite(int userId, int productId)
        {
            return _context.Favourites
                .FirstOrDefault(f => f.UserId == userId && f.ProductId == productId);
        }

        public bool AddFavourite(int userId, int productId)
        {
            // Kiểm tra đã có trong favourite chưa
            var existingFavourite = GetFavourite(userId, productId);

            if (existingFavourite != null)
            {
                return false;
            }

            var favourite = new Models.Favourite
            {
                UserId = userId,
                ProductId = productId
            };

            try
            {
                _context.Favourites.Add(favourite);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveFavourite(int favouriteId, int userId)
        {
            var favourite = _context.Favourites
                .FirstOrDefault(f => f.Id == favouriteId && f.UserId == userId);

            if (favourite == null)
            {
                return false;
            }

            try
            {
                _context.Favourites.Remove(favourite);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ToggleFavourite(int userId, int productId)
        {
            var favourite = GetFavourite(userId, productId);

            if (favourite != null)
            {
                _context.Favourites.Remove(favourite);
                _context.SaveChanges();
                return false; // Đã xóa khỏi danh sách yêu thích
            }
            else
            {
                _context.Favourites.Add(new Models.Favourite { UserId = userId, ProductId = productId });
                _context.SaveChanges();
                return true; // Đã thêm vào danh sách yêu thích
            }
        }
    }
} 