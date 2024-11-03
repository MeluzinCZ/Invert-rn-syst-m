using Inventarni_system.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Inventarni_system.Controllers
{
    public class PredmetController : Controller
    {
        private readonly AppDbContext _context;

        public PredmetController(AppDbContext context)
        {
            _context = context;
        }

        //GET: Predmet
        public async Task<IActionResult> Index()
        {
            var predmety = await _context.Predmety.Include(p => p.Sklad).ToListAsync();
            return View(predmety);
        }

        //GET: Predmet/Create
        public IActionResult Create()
        {
            ViewBag.Sklady = _context.Sklady.ToList();
            return View();
        }

        //POST: Predmet/Create
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
            ViewBag.Sklady = _context.Sklady.ToList();
            return View(predmet);
        }

        //GET: Predmet/Edit
        public async Task<IActionResult> Edit(int id)
        {
            var predmet = await _context.Predmety.FindAsync(id);
            if(predmet == null)
            {
                return NotFound();
            }
            ViewBag.Sklady = _context.Sklady.ToList();
            return View(predmet);
        }

        //POST: Predmet/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edsit(int id, Predmet predmet)
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
            ViewBag.Sklady = _context.Sklady.ToList();
            return View(predmet);
        }

        //GET: Predmet/Delete
        public async Task<IActionResult> Delete(int id)
        {
            var predmet = await _context.Predmety.Include(p => p.Sklad).FirstOrDefaultAsync(m => m.Id == id);
            if(predmet == null)
            {
                return NotFound();
            }
            return View(predmet);
        }

        //POST: Predmet/Delete
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
