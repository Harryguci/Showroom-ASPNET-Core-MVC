using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShowroomManagement.Data;
using ShowroomManagement.Models;

namespace ShowroomManagement.Controllers
{
    public class SalesTargetsController : Controller
    {
        private readonly ShowroomContext _context;
        private int listLimits = 10;

        public SalesTargetsController(ShowroomContext context)
        {
            _context = context;
        }

        // GET: SalesTargets
        public async Task<IActionResult> Index(string asc, string desc, int page = 1)
        {
            ViewBag.asc = asc;
            ViewBag.desc = desc;

            var total = _context.SalesTargets.Count();
            ViewBag.nextPage = true;
            ViewBag.totalRecord = total;
            ViewBag.totalPage = (int)Math.Ceiling((total - 1) * 1.0 / listLimits);
            ViewBag.currentPage = page;

            return _context.SalesTargets != null ?
                          View(await _context.SalesTargets
                          .Skip((page - 1) * listLimits)
                          .Take(listLimits)
                          .ToListAsync()) :
                          Problem("Entity set 'ShowroomContext.SalesTargets'  is null.");
        }

        // GET: SalesTargets/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.SalesTargets == null)
            {
                return NotFound();
            }

            var salesTarget = await _context.SalesTargets
                .FirstOrDefaultAsync(m => m.SaleId == id);
            if (salesTarget == null)
            {
                return NotFound();
            }

            return View(salesTarget);
        }

        // GET: SalesTargets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SalesTargets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SaleId,StartDate,EndDate,Total,Target,Status,Reward")] SalesTarget salesTarget)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salesTarget);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(salesTarget);
        }

        // GET: SalesTargets/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.SalesTargets == null)
            {
                return NotFound();
            }

            var salesTarget = await _context.SalesTargets.FindAsync(id);
            if (salesTarget == null)
            {
                return NotFound();
            }
            return View(salesTarget);
        }

        // POST: SalesTargets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SaleId,StartDate,EndDate,Total,Target,Status,Reward")] SalesTarget salesTarget)
        {
            if (id != salesTarget.SaleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salesTarget);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesTargetExists(salesTarget.SaleId))
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
            return View(salesTarget);
        }

        // GET: SalesTargets/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.SalesTargets == null)
            {
                return NotFound();
            }

            var salesTarget = await _context.SalesTargets
                .FirstOrDefaultAsync(m => m.SaleId == id);
            if (salesTarget == null)
            {
                return NotFound();
            }

            return View(salesTarget);
        }

        // POST: SalesTargets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.SalesTargets == null)
            {
                return Problem("Entity set 'ShowroomContext.SalesTargets'  is null.");
            }
            var salesTarget = await _context.SalesTargets.FindAsync(id);
            if (salesTarget != null)
            {
                _context.SalesTargets.Remove(salesTarget);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesTargetExists(string id)
        {
            return (_context.SalesTargets?.Any(e => e.SaleId == id)).GetValueOrDefault();
        }
    }
}
