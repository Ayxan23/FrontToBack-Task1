
using Microsoft.AspNetCore.Hosting;

namespace FrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SliderController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Slider> sliders = _context.Sliders.AsEnumerable();

            ViewBag.Count = sliders.Count();

            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SliderViewModel sliderViewModel)
        {
            if (!ModelState.IsValid)
                return View();

            if (sliderViewModel.Image == null)
            {
                ModelState.AddModelError("Image", "Image bos olmamalidir.");
                return View();
            }
            //sliderViewModel.Image.FileName
            //sliderViewModel.Image.Length.ToString
            //sliderViewModel.Image.ContentType;           
            if (sliderViewModel.Image.Length / 1024 > 100)
            {
                ModelState.AddModelError("Image", "Faylin hecmi 100 kb-dan boyuk olmamalidir.");
                return View();
            }
            if (!sliderViewModel.Image.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Image", "Faylin tipi image olmalidir.");
                return View();
            }

            //string path = _webHostEnvironment.WebRootPath + @"\assets\images\website-images" + sliderViewModel.Image.FileName;
            string fileName = $"{Guid.NewGuid()}-{sliderViewModel.Image.FileName}";
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", fileName);
            using (FileStream stream = new(path, FileMode.Create))
            {
                await sliderViewModel.Image.CopyToAsync(stream);
                //stream.Dispose();
            }

            Slider slider = new()
            {
                Title = sliderViewModel.Title,
                Description = sliderViewModel.Description,
                Offer = sliderViewModel.Offer,
                Image = fileName
            };

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }






        public IActionResult Read(int id)
        {

            Slider? slider = _context.Sliders.FirstOrDefault(i => i.Id == id);
            if (slider is null)
                return NotFound();

            return View(slider);
        }

        public IActionResult Delete(int id)
        {
            Slider? slider = _context.Sliders.FirstOrDefault(i => i.Id == id);
            if (slider is null)
                return NotFound();

            return View(slider);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteSlider(int id)
        {
            Slider? slider = _context.Sliders.FirstOrDefault(i => i.Id == id);
            if (slider is null)
                return NotFound();

            _context.Sliders.Remove(slider);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            Slider? slider = _context.Sliders.FirstOrDefault(i => i.Id == id);
            if (slider is null)
                return NotFound();

            return View(slider);
        }

        [HttpPost]
        public IActionResult Update(Slider slider, int id)
        {
            Slider? dbInfo = _context.Sliders.AsNoTracking().FirstOrDefault(i => i.Id == id);
            if (slider is null)
                return NotFound();

            try
            {
                _context.Sliders.SingleOrDefault(i => i.Title == slider.Title);
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError("Title", "This title already exists");
                return View();
            }

            _context.Sliders.Update(slider);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
