using Inventarni_system.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        //GET: Budova
        public async Task<IActionResult> Index()
        {
            var budovy = await _context.Budovy.ToListAsync();
            return View(budovy);
        }

        // GET: Budova/Create
        public IActionResult Create()
        {
            return View(new Budova());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Budova budova)
        {
            if (string.IsNullOrWhiteSpace(budova.Nazev) || string.IsNullOrWhiteSpace(budova.Typ))
            {
                budova.Nazev = "Test Nazev"; // Přidání testovací hodnoty
                budova.Typ = "Test Typ"; // Přidání testovací hodnoty
            }

            if (ModelState.IsValid)
            {
                _context.Budovy.Add(budova);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(budova);
        }



        //GET: Budova/Edit
        public async Task<IActionResult> Edit(int id)
        {
            var budova = await _context.Budovy.FindAsync(id);
            if(budova == null)
            {
                return NotFound();
            }
            return View(budova);
        }

        //POST: Budova/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Budova budova)
        {
            if (id != budova.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(budova);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudovaExists(budova.Id))
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
            return View(budova);
        }

        //GET: Budova/Delete
        public async Task<IActionResult> Delete(int id)
        {
            var budova = await _context.Budovy.FindAsync(id);
            if (budova == null)
            {
                return NotFound();
            }
            return View(budova);
        }

        //POST: Budova/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var budova = await _context.Budovy.FindAsync(id);
            _context.Budovy.Remove(budova);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BudovaExists(int id)
        {
            return _context.Budovy.Any(e => e.Id == id);
        }
    }
}
