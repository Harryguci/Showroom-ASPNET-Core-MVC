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
    public class TestDrivesController : Controller
    {
        private readonly ShowroomContext _context;

        public TestDrivesController(ShowroomContext context)
        {
            _context = context;
        }

        // GET: TestDrives
        public async Task<IActionResult> Index()
        {
              return _context.TestDrives != null ? 
                          View(await _context.TestDrives.ToListAsync()) :
                          Problem("Entity set 'ShowroomContext.TestDrives'  is null.");
        }

        // GET: TestDrives/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.TestDrives == null)
            {
                return NotFound();
            }

            var testDrive = await _context.TestDrives
                .FirstOrDefaultAsync(m => m.DriveId == id);
            if (testDrive == null)
            {
                return NotFound();
            }

            return View(testDrive);
        }

        // GET: TestDrives/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TestDrives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DriveId,ClientId,BookDate,Note,Status")] TestDrive testDrive)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testDrive);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testDrive);
        }

        // GET: TestDrives/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.TestDrives == null)
            {
                return NotFound();
            }

            var testDrive = await _context.TestDrives.FindAsync(id);
            if (testDrive == null)
            {
                return NotFound();
            }
            return View(testDrive);
        }

        // POST: TestDrives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("DriveId,ClientId,BookDate,Note,Status")] TestDrive testDrive)
        {
            if (id != testDrive.DriveId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testDrive);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestDriveExists(testDrive.DriveId))
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
            return View(testDrive);
        }

        // GET: TestDrives/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.TestDrives == null)
            {
                return NotFound();
            }

            var testDrive = await _context.TestDrives
                .FirstOrDefaultAsync(m => m.DriveId == id);
            if (testDrive == null)
            {
                return NotFound();
            }

            return View(testDrive);
        }

        // POST: TestDrives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.TestDrives == null)
            {
                return Problem("Entity set 'ShowroomContext.TestDrives'  is null.");
            }
            var testDrive = await _context.TestDrives.FindAsync(id);
            if (testDrive != null)
            {
                _context.TestDrives.Remove(testDrive);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestDriveExists(string id)
        {
          return (_context.TestDrives?.Any(e => e.DriveId == id)).GetValueOrDefault();
        }
    }
}
