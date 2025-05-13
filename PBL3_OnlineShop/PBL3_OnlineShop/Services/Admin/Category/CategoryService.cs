using AspNetCoreGeneratedDocument;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Data;

namespace PBL3_OnlineShop.Services.Admin.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly PBL3_Db_Context _context;
        public CategoryService(PBL3_Db_Context context)
        {
            _context = context;
        }
        public List<Models.Category> GetAllCategories()
        {
            return _context.Categories.OrderByDescending(p => p.CategoryId).ToList();
        }

        public Models.Category GetCategoryById(int id)
        {
            return _context.Categories.FirstOrDefault(p => p.CategoryId == id);
        }

        public bool CreateCategory(Models.Category category)
        {
            if( _context.Categories.Any(p => p.CategoryName == category.CategoryName))
            {
                return false;
            }
            _context.Add(category);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(p => p.CategoryId == id);
            if (category == null)
            {
                return false;
            }
            _context.Remove(category);
            _context.SaveChanges();
            return true;
        }


        public bool UpdateCategory(Models.Category category)
        {
            var categoryToUpdate = _context.Categories.FirstOrDefault(p => p.CategoryId == category.CategoryId);
            if (categoryToUpdate == null)
            {
                return false;
            }
            categoryToUpdate.CategoryName = category.CategoryName;
            categoryToUpdate.Description = category.Description;
            categoryToUpdate.Status = category.Status;
            _context.Update(categoryToUpdate);
            _context.SaveChanges();
            return true;
        }
    }
}
