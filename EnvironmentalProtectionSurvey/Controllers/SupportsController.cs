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
    public class SupportsController : Controller
    {
        private readonly Project2Context _context;

        public SupportsController(Project2Context context)
        {
            _context = context;
        }

        // GET: Supports
        public async Task<IActionResult> Index()
        {
              return _context.Supports != null ? 
                          View(await _context.Supports.ToListAsync()) :
                          Problem("Entity set 'Project2Context.Supports'  is null.");
        }

        // GET: Supports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Supports == null)
            {
                return NotFound();
            }

            var support = await _context.Supports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (support == null)
            {
                return NotFound();
            }

            return View(support);
        }

        // GET: Supports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Supports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,TextMessage")] Support support)
        {
            if (ModelState.IsValid)
            {
                _context.Add(support);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(support);
        }

        // GET: Supports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Supports == null)
            {
                return NotFound();
            }

            var support = await _context.Supports.FindAsync(id);
            if (support == null)
            {
                return NotFound();
            }
            return View(support);
        }

        // POST: Supports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,TextMessage")] Support support)
        {
            if (id != support.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(support);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupportExists(support.Id))
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
            return View(support);
        }

        // GET: Supports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Supports == null)
            {
                return NotFound();
            }

            var support = await _context.Supports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (support == null)
            {
                return NotFound();
            }

            return View(support);
        }

        // POST: Supports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Supports == null)
            {
                return Problem("Entity set 'Project2Context.Supports'  is null.");
            }
            var support = await _context.Supports.FindAsync(id);
            if (support != null)
            {
                _context.Supports.Remove(support);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupportExists(int id)
        {
          return (_context.Supports?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
