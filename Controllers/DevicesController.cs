using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using ShowroomManagement.Data;
using ShowroomManagement.Models;

namespace ShowroomManagement.Controllers
{
    public class DevicesController : Controller
    {
        private readonly ShowroomContext _context;
        private int listLimits = 10;

        public DevicesController(ShowroomContext context)
        {
            _context = context;
        }

        // GET: Devices
        public async Task<IActionResult> Index(string asc, string desc, int page = 1)
        {
            ViewBag.asc = asc;
            ViewBag.desc = desc;

            var total = _context.Customer.Count();
            ViewBag.nextPage = true;
            ViewBag.totalRecord = total;
            ViewBag.totalPage = (int)Math.Ceiling((total - 1) * 1.0 / listLimits);
            ViewBag.currentPage = page;

            return _context.Devices != null ? 
                          View(await _context.Devices.ToListAsync()) :
                          Problem("Entity set 'ShowroomContext.Devices'  is null.");
        }

        // GET: Devices/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Devices == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .FirstOrDefaultAsync(m => m.DeviceId == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // GET: Devices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Devices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeviceId,DeviceName,DateEntry,Status,Price")] Device device)
        {
            if (ModelState.IsValid)
            {
                _context.Add(device);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(device);
        }

        // GET: Devices/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Devices == null)
            {
                return NotFound();
            }

            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }
            return View(device);
        }

        // POST: Devices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("DeviceId,DeviceName,DateEntry,Status,Price")] Device device)
        {
            if (id != device.DeviceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(device);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceExists(device.DeviceId))
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
            return View(device);
        }

        // GET: Devices/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Devices == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .FirstOrDefaultAsync(m => m.DeviceId == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Devices == null)
            {
                return Problem("Entity set 'ShowroomContext.Devices'  is null.");
            }
            var device = await _context.Devices.FindAsync(id);
            if (device != null)
            {
                _context.Devices.Remove(device);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceExists(string id)
        {
          return (_context.Devices?.Any(e => e.DeviceId == id)).GetValueOrDefault();
        }
    }
}
