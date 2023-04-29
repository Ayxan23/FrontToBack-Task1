using FrontToBack.Contexts;
using FrontToBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InfoController : Controller
    {
        private readonly AppDbContext _context;

        public InfoController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Info> infos = _context.Infos.ToList();
            
            ViewBag.Count = infos.Count;

            return View(infos);
        }

        public IActionResult Create()
        {
            //if (_context.Infos.Count() == 3)
            //    return BadRequest();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Info info)
        {
            if (!ModelState.IsValid)
                return View();        

            try
            {
                _context.Infos.SingleOrDefault(i => i.Title == info.Title);
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError("Title", "This title already exists");
                return View();
            }

            _context.Infos.Add(info);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Read(int id)
        {
            //_context.Infos.Find(id);
            Info? info = _context.Infos.FirstOrDefault(i => i.Id == id);
            if (info is null)
                return NotFound();

            return View(info);
        }

        public IActionResult Delete(int id)
        {
            Info? info = _context.Infos.FirstOrDefault(i => i.Id == id);
            if (info is null)
                return NotFound();

            return View(info);
        }

        [HttpPost]
        [ActionName ("Delete")]
        public IActionResult DeleteInfo(int id)
        {
            Info? info = _context.Infos.FirstOrDefault(i => i.Id == id);
            if (info is null)
                return NotFound();

            _context.Infos.Remove(info);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            Info? info = _context.Infos.FirstOrDefault(i => i.Id == id);
            if (info is null)
                return NotFound();

            return View(info);
        }

        [HttpPost]
        public IActionResult Update(Info info, int id)
        {
            Info? dbInfo = _context.Infos.AsNoTracking().FirstOrDefault(i => i.Id == id);
            if (info is null)
                return NotFound();

            try
            {
                _context.Infos.SingleOrDefault(i => i.Title == info.Title);
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError("Title", "This title already exists");
                return View();
            }

            _context.Infos.Update(info);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
