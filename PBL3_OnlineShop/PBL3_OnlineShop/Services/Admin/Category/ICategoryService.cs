namespace PBL3_OnlineShop.Services.Admin.Category
{
    public interface ICategoryService
    {
        public List<Models.Category> GetAllCategories();
        public Models.Category GetCategoryById(int id);
        public bool CreateCategory(Models.Category category);
        public bool UpdateCategory(Models.Category category);
        public bool DeleteCategory(int id);
    }
}
