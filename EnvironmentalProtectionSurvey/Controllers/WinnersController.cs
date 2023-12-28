using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EnvironmentalProtectionSurvey.Models;

namespace EnvironmentalProtectionSurvey.Controllers
{
    public class WinnersController : Controller
    {
        private readonly Project2Context _context;

        public WinnersController(Project2Context context)
        {
            _context = context;
        }

        // GET: Winners
        public async Task<IActionResult> Index()
        {
            var project2Context = _context.Winners.Include(w => w.Contest).Include(w => w.User);
            return View(await project2Context.ToListAsync());
        }

        // GET: Winners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Winners == null)
            {
                return NotFound();
            }

            var winner = await _context.Winners
                .Include(w => w.Contest)
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (winner == null)
            {
                return NotFound();
            }

            return View(winner);
        }

        // GET: Winners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Winners == null)
            {
                return NotFound();
            }

            var winner = await _context.Winners
                .Include(w => w.Contest)
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (winner == null)
            {
                return NotFound();
            }

            return View(winner);
        }

        // POST: Winners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Winners == null)
            {
                return Problem("Entity set 'Project2Context.Winners'  is null.");
            }
            var winner = await _context.Winners.FindAsync(id);
            if (winner != null)
            {
                _context.Winners.Remove(winner);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WinnerExists(int id)
        {
          return (_context.Winners?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
