using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Repository;

namespace PBL3_OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly PBL3_Db_Context _context;

        public CategoryController(PBL3_Db_Context context)
        {
            _context = context;
        }
        public async Task<IActionResult> IndexAsync()
        {
            return View(await _context.Categories
            .OrderByDescending(p => p.CategoryId)
            .ToListAsync());
        }
    }
}
