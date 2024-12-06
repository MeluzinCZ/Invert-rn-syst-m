using Inventarni_system.Data;
using Inventarni_system.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Inventarni_system.Controllers
{
    public class BudovaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BudovaController> _logger;

        public BudovaController(AppDbContext context, ILogger<BudovaController> logger)
        {
            _context = context;
            _logger = logger;
        }


        // GET: Budova
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var budovy = from b in _context.Budovy
                         select b;

            if (!string.IsNullOrEmpty(searchString))
            {
                string loweredSearch = searchString.ToLower();

                budovy = budovy.Where(b =>
                    b.Nazev.ToLower().Contains(loweredSearch) ||
                    (b.Typ != null && b.Typ.ToLower().Contains(loweredSearch)));
            }

            try
            {
                var result = await budovy.ToListAsync();
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Chyba při načítání seznamu budov.");
                return View("Error");
            }
        }

        // GET: Budova/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Budova/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazev,Typ")] Budova budova)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(budova);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Nová budova byla vytvořena: {Nazev}", budova.Nazev);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Chyba při vytváření nové budovy.");
                    ModelState.AddModelError(string.Empty, "Došlo k chybě při vytváření budovy. Zkuste to prosím znovu.");
                }
            }

            // Logování chyb z ModelState
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _logger.LogError("ModelState error: {ErrorMessage}", error.ErrorMessage);
                }
            }

            return View(budova);
        }

        // GET: Budova/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit akce byla zavolána bez ID.");
                return NotFound();
            }

            var budova = await _context.Budovy.FindAsync(id);
            if (budova == null)
            {
                _logger.LogWarning("Nebyla nalezena budova s ID {Id}.", id);
                return NotFound();
            }
            return View(budova);
        }

        // POST: Budova/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazev,Typ")] Budova budova)
        {
            if (id != budova.Id)
            {
                _logger.LogWarning("Edit akce: ID v URL ({UrlId}) se neshoduje s ID v modelu ({ModelId}).", id, budova.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(budova);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Budova s ID {Id} byla aktualizována.", budova.Id);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!BudovaExists(budova.Id))
                    {
                        _logger.LogWarning("Nebyla nalezena budova s ID {Id} během aktualizace.", budova.Id);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "Chyba při aktualizaci budovy s ID {Id}.", budova.Id);
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Chyba při aktualizaci budovy s ID {Id}.", budova.Id);
                    ModelState.AddModelError(string.Empty, "Došlo k chybě při aktualizaci budovy. Zkuste to prosím znovu.");
                }
            }
            return View(budova);
        }

        // GET: Budova/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete akce byla zavolána bez ID.");
                return NotFound();
            }

            var budova = await _context.Budovy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (budova == null)
            {
                _logger.LogWarning("Nebyla nalezena budova s ID {Id} při načítání pro smazání.", id);
                return NotFound();
            }

            return View(budova);
        }

        // POST: Budova/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var budova = await _context.Budovy.FindAsync(id);
            if (budova != null)
            {
                try
                {
                    _context.Budovy.Remove(budova);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Budova s ID {Id} byla smazána.", id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Chyba při mazání budovy s ID {Id}.", id);
                    ModelState.AddModelError(string.Empty, "Došlo k chybě při mazání budovy. Zkuste to prosím znovu.");
                    return View(budova);
                }
            }
            else
            {
                _logger.LogWarning("Budova s ID {Id} nebyla nalezena při pokusu o smazání.", id);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BudovaExists(int id)
        {
            return _context.Budovy.Any(e => e.Id == id);
        }
    }
}
