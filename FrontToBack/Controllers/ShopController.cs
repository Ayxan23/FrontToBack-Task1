using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FrontToBack.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? catecoryId)
        {
            IQueryable<Product> products = _context.Products.Where(p => !p.IsDeleted).AsQueryable(); //.IgnoreQueryFilters()
            ViewBag.ProductsCount = await products.CountAsync();

            ShopViewModel shopViewModel = new()
            {
                Products = catecoryId != null 
                ? await products.Where(p => p.CategoryId == catecoryId).ToListAsync()
                : await products.ToListAsync(),
                Categories = await _context.Categories.Include(c => c.Products.Where(p => !p.IsDeleted)).Where(c => !c.IsDeleted).ToListAsync()
            };

            return View(shopViewModel);
        }
    }
}