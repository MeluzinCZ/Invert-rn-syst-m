using System;
using System.Linq;
using System.Threading.Tasks;
using Inventarni_system.Data;
using Inventarni_system.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inventarni_system.Controllers
{
    public class SkladController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SkladController> _logger;

        public SkladController(AppDbContext context, ILogger<SkladController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Sklad
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var sklady = from s in _context.Sklady.Include(s => s.Budova)
                         select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                string loweredSearch = searchString.ToLower();

                sklady = sklady.Where(s =>
                    s.Nazev.ToLower().Contains(loweredSearch) ||
                    (s.Budova != null && s.Budova.Nazev != null && s.Budova.Nazev.ToLower().Contains(loweredSearch)));
            }

            try
            {
                var result = await sklady.ToListAsync();
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Chyba při načítání seznamu skladů.");
                return View("Error");
            }
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
        public async Task<IActionResult> Create([Bind("Nazev,BudovaId")] Sklad sklad)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(sklad);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Nový sklad byl vytvořen: {Nazev}", sklad.Nazev);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Chyba při vytváření nového skladu.");
                    ModelState.AddModelError(string.Empty, "Došlo k chybě při vytváření skladu. Zkuste to prosím znovu.");
                }
            }

            ViewData["BudovaId"] = new SelectList(_context.Budovy, "Id", "Nazev", sklad.BudovaId);
            // Logování chyb z ModelState
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _logger.LogError("ModelState error: {ErrorMessage}", error.ErrorMessage);
                }
            }

            return View(sklad);
        }

        // GET: Sklad/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit akce byla zavolána bez ID.");
                return NotFound();
            }

            var sklad = await _context.Sklady.FindAsync(id);
            if (sklad == null)
            {
                _logger.LogWarning("Nebyla nalezena sklad s ID {Id}.", id);
                return NotFound();
            }
            ViewData["BudovaId"] = new SelectList(_context.Budovy, "Id", "Nazev", sklad.BudovaId);
            return View(sklad);
        }

        // POST: Sklad/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazev,BudovaId")] Sklad sklad)
        {
            if (id != sklad.Id)
            {
                _logger.LogWarning("Edit akce: ID v URL ({UrlId}) se neshoduje s ID v modelu ({ModelId}).", id, sklad.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sklad);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Sklad s ID {Id} byl aktualizován.", sklad.Id);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!SkladExists(sklad.Id))
                    {
                        _logger.LogWarning("Nebyla nalezena sklad s ID {Id} během aktualizace.", sklad.Id);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "Chyba při aktualizaci skladu s ID {Id}.", sklad.Id);
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Chyba při aktualizaci skladu s ID {Id}.", sklad.Id);
                    ModelState.AddModelError(string.Empty, "Došlo k chybě při aktualizaci skladu. Zkuste to prosím znovu.");
                }
            }

            ViewData["BudovaId"] = new SelectList(_context.Budovy, "Id", "Nazev", sklad.BudovaId);
            // Logování chyb z ModelState
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _logger.LogError("ModelState error: {ErrorMessage}", error.ErrorMessage);
                }
            }

            return View(sklad);
        }

        // GET: Sklad/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete akce byla zavolána bez ID.");
                return NotFound();
            }

            var sklad = await _context.Sklady
                .Include(s => s.Budova)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sklad == null)
            {
                _logger.LogWarning("Nebyla nalezena sklad s ID {Id} při načítání pro smazání.", id);
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
            if (sklad != null)
            {
                try
                {
                    _context.Sklady.Remove(sklad);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Sklad s ID {Id} byl smazán.", id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Chyba při mazání skladu s ID {Id}.", id);
                    ModelState.AddModelError(string.Empty, "Došlo k chybě při mazání skladu. Zkuste to prosím znovu.");
                    return View(sklad);
                }
            }
            else
            {
                _logger.LogWarning("Sklad s ID {Id} nebyl nalezen při pokusu o smazání.", id);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SkladExists(int id)
        {
            return _context.Sklady.Any(e => e.Id == id);
        }
    }
}
