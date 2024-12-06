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

            var predmety = from p in _context.Predmety.Include(p => p.Sklad)
                           select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                string loweredSearch = searchString.ToLower();

                predmety = predmety.Where(p =>
                    p.Nazev.ToLower().Contains(loweredSearch) ||
                    p.Sklad.Nazev.ToLower().Contains(loweredSearch));
            }

            try
            {
                var result = await predmety.ToListAsync();
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Chyba při načítání seznamu předmětů.");
                return View("Error");
            }
        }

        // GET: Predmet/Create
        public IActionResult Create()
        {
            ViewData["SkladId"] = new SelectList(_context.Sklady, "Id", "Nazev");
            return View();
        }

        // POST: Predmet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nazev,Mnozstvi,CenaZaKus,SkladId")] Predmet predmet)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(predmet);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Nový předmět byl vytvořen: {Nazev}", predmet.Nazev);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Chyba při vytváření nového předmětu.");
                    ModelState.AddModelError(string.Empty, "Došlo k chybě při vytváření předmětu. Zkuste to prosím znovu.");
                }
            }

            ViewData["SkladId"] = new SelectList(_context.Sklady, "Id", "Nazev", predmet.SkladId);
            // Logování chyb z ModelState
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _logger.LogError("ModelState error: {ErrorMessage}", error.ErrorMessage);
                }
            }

            return View(predmet);
        }

        // GET: Predmet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit akce byla zavolána bez ID.");
                return NotFound();
            }

            var predmet = await _context.Predmety.FindAsync(id);
            if (predmet == null)
            {
                _logger.LogWarning("Nebyl nalezen předmět s ID {Id}.", id);
                return NotFound();
            }
            ViewData["SkladId"] = new SelectList(_context.Sklady, "Id", "Nazev", predmet.SkladId);
            return View(predmet);
        }

        // POST: Predmet/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazev,Mnozstvi,CenaZaKus,SkladId")] Predmet predmet)
        {
            if (id != predmet.Id)
            {
                _logger.LogWarning("Edit akce: ID v URL ({UrlId}) se neshoduje s ID v modelu ({ModelId}).", id, predmet.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(predmet);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Předmět s ID {Id} byl aktualizován.", predmet.Id);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!PredmetExists(predmet.Id))
                    {
                        _logger.LogWarning("Nebyl nalezen předmět s ID {Id} během aktualizace.", predmet.Id);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "Chyba při aktualizaci předmětu s ID {Id}.", predmet.Id);
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Chyba při aktualizaci předmětu s ID {Id}.", predmet.Id);
                    ModelState.AddModelError(string.Empty, "Došlo k chybě při aktualizaci předmětu. Zkuste to prosím znovu.");
                }
            }

            ViewData["SkladId"] = new SelectList(_context.Sklady, "Id", "Nazev", predmet.SkladId);
            // Logování chyb z ModelState
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _logger.LogError("ModelState error: {ErrorMessage}", error.ErrorMessage);
                }
            }

            return View(predmet);
        }

        // GET: Predmet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete akce byla zavolána bez ID.");
                return NotFound();
            }

            var predmet = await _context.Predmety
                .Include(p => p.Sklad)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (predmet == null)
            {
                _logger.LogWarning("Nebyl nalezen předmět s ID {Id}.", id);
                return NotFound();
            }

            return View(predmet);
        }

        // POST: Predmet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var predmet = await _context.Predmety.FindAsync(id);
            if (predmet != null)
            {
                try
                {
                    _context.Predmety.Remove(predmet);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Předmět s ID {Id} byl smazán.", id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Chyba při mazání předmětu s ID {Id}.", id);
                    ModelState.AddModelError(string.Empty, "Došlo k chybě při mazání předmětu. Zkuste to prosím znovu.");
                    return View(predmet);
                }
            }
            else
            {
                _logger.LogWarning("Předmět s ID {Id} nebyl nalezen při pokusu o smazání.", id);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PredmetExists(int id)
        {
            return _context.Predmety.Any(e => e.Id == id);
        }
    }
}
