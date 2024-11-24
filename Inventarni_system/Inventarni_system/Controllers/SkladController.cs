using Inventarni_system.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Inventarni_system.Controllers
{
    public class SkladController : Controller
    {
        private readonly AppDbContext _context;

        public SkladController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Sklad
        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            IQueryable<Sklad> sklady = _context.Sklady.Include(s => s.Budova);

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                sklady = sklady.Where(s => s.Nazev.ToLower().Contains(searchString) || s.Budova.Nazev.ToLower().Contains(searchString));
            }

            return View(await sklady.ToListAsync());
        }


        // GET: Sklad/Create
        public IActionResult Create()
        {
            ViewData["BudovaId"] = new SelectList(_context.Budovy, "Id", "Nazev");
            return View();
        }

        // POST: Sklad/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sklad sklad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sklad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BudovaId"] = new SelectList(_context.Budovy, "Id", "Nazev", sklad.BudovaId);
            return View(sklad);
        }

        // GET: Sklad/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var sklad = await _context.Sklady.FindAsync(id);
            if (sklad == null)
            {
                return NotFound();
            }
            ViewData["BudovaId"] = new SelectList(_context.Budovy, "Id", "Nazev", sklad.BudovaId);
            return View(sklad);
        }

        // POST: Sklad/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Sklad sklad)
        {
            if (id != sklad.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sklad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Sklady.Any(e => e.Id == sklad.Id))
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
            ViewData["BudovaId"] = new SelectList(_context.Budovy, "Id", "Nazev", sklad.BudovaId);
            return View(sklad);
        }

        // GET: Sklad/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var sklad = await _context.Sklady
                .Include(s => s.Budova)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sklad == null)
            {
                return NotFound();
            }

            return View(sklad);
        }

        // POST: Sklad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sklad = await _context.Sklady.FindAsync(id);
            _context.Sklady.Remove(sklad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
