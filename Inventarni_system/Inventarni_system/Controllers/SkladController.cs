using Inventarni_system.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Inventarni_system.Controllers
{
    public class SkladController : Controller
    {
        private readonly AppDbContext _context;

        public SkladController(AppDbContext context)
        {
            _context = context;
        }

        //GET: Sklad
        public async Task<IActionResult> Index()
        {
            var sklady = await _context.Sklady.Include(s => s.Budova).ToListAsync();
            return View(sklady);
        }

        //GET: Sklad/Create
        public IActionResult Create()
        {
            ViewBag.Budovy = _context.Budovy.ToList();
            return View();
        }

        //POST: Sklad/Create 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sklad sklad)
        {
            if (ModelState.IsValid)
            {
                _context.Sklady.Add(sklad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Budovy = _context.Budovy.ToList();
            return View(sklad);
        }

        //GET: Sklad/Edit
        public async Task<IActionResult> Edit(int id)
        {
            var sklad = await _context.Sklady.FindAsync(id);
            if(sklad == null)
            {
                return NotFound();
            }
            ViewBag.Budovy = _context.Budovy.ToList();
            return View(sklad);
        }

        //POST: Sklad/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Sklad sklad)
        {
            if(id != sklad.Id)
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
                    if (!SkladExists(sklad.Id))
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
            ViewBag.Budovy = _context.Budovy.ToList();
            return View(sklad);
        }

        //GET: Sklad/Delete
        public async Task<IActionResult> Delete(int id)
        {
            var sklad = await _context.Sklady.FindAsync(id);
            if (sklad == null)
            {
                return NotFound();
            }
            return View(sklad);
        }

        //POST: Sklad/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sklad = await _context.Sklady.FindAsync(id);
            _context.Sklady.Remove(sklad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SkladExists(int id)
        {
            return _context.Sklady.Any(e => e.Id == id);
        }
    }
}
