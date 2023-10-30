using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShowroomManagement.Data;
using ShowroomManagement.Models;

namespace ShowroomManagement.Controllers
{
    [Authorize]
    public class SourcesController : Controller
    {
        private readonly ShowroomContext _context;
        private int listLimits = 10;

        public SourcesController(ShowroomContext context)
        {
            _context = context;
        }

        // GET: Sources
        public async Task<IActionResult> Index(string asc, string desc, int page = 1)
        {
            ViewBag.asc = asc;
            ViewBag.desc = desc;

            var total = _context.Source.Count();
            ViewBag.nextPage = true;
            ViewBag.totalRecord = total;
            ViewBag.totalPage = (int)Math.Ceiling((total - 1) * 1.0 / listLimits);
            ViewBag.currentPage = page;

            return _context.Source != null ? 
                          View(await _context.Source
                          .Skip((page - 1) * listLimits)
                          .Take(listLimits)
                          .ToListAsync()) :
                          Problem("Entity set 'ShowroomContext.Source'  is null.");
        }

        // GET: Sources/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Source == null)
            {
                return NotFound();
            }

            var source = await _context.Source
                .FirstOrDefaultAsync(m => m.SourceId == id);
            if (source == null)
            {
                return NotFound();
            }

            return View(source);
        }

        // GET: Sources/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sources/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SourceId,Name")] Source source)
        {
            if (ModelState.IsValid)
            {
                _context.Add(source);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(source);
        }

        // GET: Sources/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Source == null)
            {
                return NotFound();
            }

            var source = await _context.Source.FindAsync(id);
            if (source == null)
            {
                return NotFound();
            }
            return View(source);
        }

        // POST: Sources/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SourceId,Name")] Source source)
        {
            if (id != source.SourceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(source);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SourceExists(source.SourceId))
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
            return View(source);
        }

        // GET: Sources/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Source == null)
            {
                return NotFound();
            }

            var source = await _context.Source
                .FirstOrDefaultAsync(m => m.SourceId == id);
            if (source == null)
            {
                return NotFound();
            }

            return View(source);
        }

        // POST: Sources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Source == null)
            {
                return Problem("Entity set 'ShowroomContext.Source'  is null.");
            }
            var source = await _context.Source.FindAsync(id);
            if (source != null)
            {
                _context.Source.Remove(source);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SourceExists(string id)
        {
          return (_context.Source?.Any(e => e.SourceId == id)).GetValueOrDefault();
        }
    }
}
