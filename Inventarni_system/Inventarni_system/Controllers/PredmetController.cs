using Inventarni_system.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventarni_system.Controllers
{
    public class PredmetController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PredmetController> _logger;

        public PredmetController(AppDbContext context, ILogger<PredmetController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Predmet
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            IQueryable<Predmet> predmety = _context.Predmety.Include(p => p.Sklad);

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                predmety = predmety.Where(p => p.Nazev.ToLower().Contains(searchString) || p.SkladId.ToString().Contains(searchString) || p.Sklad.Nazev.ToLower().Contains(searchString));
            }

            return View(await predmety.ToListAsync());
        }

        // GET: Predmet/Create
        public IActionResult Create()
        {
            return View(new Predmet());
        }

        // POST: Predmet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Predmet predmet)
        {
            if (ModelState.IsValid)
            {
                _context.Predmety.Add(predmet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(predmet);
        }

        // GET: Predmet/Edit
        public async Task<IActionResult> Edit(int id)
        {
            var predmet = await _context.Predmety.FindAsync(id);
            if (predmet == null)
            {
                return NotFound();
            }
            return View(predmet);
        }

        // POST: Predmet/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Predmet predmet)
        {
            if (id != predmet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(predmet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PredmetExists(predmet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(predmet);
        }

        // GET: Predmet/Delete
        public async Task<IActionResult> Delete(int id)
        {
            var predmet = await _context.Predmety.FindAsync(id);
            if (predmet == null)
            {
                return NotFound();
            }
            return View(predmet);
        }

        // POST: Predmet/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var predmet = await _context.Predmety.FindAsync(id);
            _context.Predmety.Remove(predmet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PredmetExists(int id)
        {
            return _context.Predmety.Any(e => e.Id == id);
        }
    }
}
