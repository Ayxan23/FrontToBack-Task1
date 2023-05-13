using Microsoft.AspNetCore.Mvc;

namespace FrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(p => p.Category)
                .Where(p => !p.IsDeleted).OrderByDescending(p => p.ModifiedAt).ToListAsync();

            return View(products);
        }


        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.AsEnumerable().Where(c => !c.IsDeleted);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            ViewBag.Categories = _context.Categories.AsEnumerable().Where(c => !c.IsDeleted);

            if (!ModelState.IsValid)
                return View();

            if (!_context.Categories.Any(c => c.Id == productViewModel.CategoryId))
                return BadRequest();

            Product product = new()
            {
                Name = productViewModel.Name,
                Price = productViewModel.Price,
                Raiting = productViewModel.Raiting,
                Image = productViewModel.Image,
                CategoryId = productViewModel.CategoryId,
                IsDeleted = false
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int id)
        {
            Product? product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (product is null)
                return NotFound();

            ViewBag.Categories = _context.Categories.Where(c => !c.IsDeleted);

            ProductViewModel productViewModel = new()
            {
                Id = product.Id,
                Name = product.Name,
                Image = product.Image,
                Price = product.Price,
                Raiting = product.Raiting,
                CategoryId = product.CategoryId,
            };

            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, ProductViewModel productViewModel)
        {
            ViewBag.Categories = _context.Categories.Where(c => !c.IsDeleted);

            if (!ModelState.IsValid)
                return View();

            if (!_context.Categories.Any(c => c.Id == productViewModel.CategoryId))
                return BadRequest();

            Product? product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (product is null)
                return NotFound();

            product.Name = productViewModel.Name;
            product.Price = productViewModel.Price;
            product.Raiting = productViewModel.Raiting;
            product.CategoryId = productViewModel.CategoryId;
            product.Image = productViewModel.Image;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (product is null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (product is null)
                return NotFound();

            product.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Read(int id)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (product is null)
                return NotFound();

            return View(product);
        }
    }
}
