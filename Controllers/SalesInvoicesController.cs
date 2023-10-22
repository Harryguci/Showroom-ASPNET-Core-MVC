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
    public class SalesInvoicesController : Controller
    {
        private readonly ShowroomContext _context;

        public SalesInvoicesController(ShowroomContext context)
        {
            _context = context;
        }

        // GET: SalesInvoices
        public async Task<IActionResult> Index()
        {
              return _context.SalesInvoices != null ? 
                          View(await _context.SalesInvoices.ToListAsync()) :
                          Problem("Entity set 'ShowroomContext.SalesInvoices'  is null.");
        }

        // GET: SalesInvoices/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.SalesInvoices == null)
            {
                return NotFound();
            }

            var salesInvoice = await _context.SalesInvoices
                .FirstOrDefaultAsync(m => m.InSaleId == id);
            if (salesInvoice == null)
            {
                return NotFound();
            }

            return View(salesInvoice);
        }

        // GET: SalesInvoices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SalesInvoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InSalesId,ClientId,SaleDate,Status,QuantitySale")] SalesInvoice salesInvoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salesInvoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(salesInvoice);
        }

        // GET: SalesInvoices/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.SalesInvoices == null)
            {
                return NotFound();
            }

            var salesInvoice = await _context.SalesInvoices.FindAsync(id);
            if (salesInvoice == null)
            {
                return NotFound();
            }
            return View(salesInvoice);
        }

        // POST: SalesInvoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("InSalesId,ClientId,SaleDate,Status,QuantitySale")] SalesInvoice salesInvoice)
        {
            if (id != salesInvoice.InSaleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salesInvoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesInvoiceExists(salesInvoice.InSaleId))
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
            return View(salesInvoice);
        }

        // GET: SalesInvoices/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.SalesInvoices == null)
            {
                return NotFound();
            }

            var salesInvoice = await _context.SalesInvoices
                .FirstOrDefaultAsync(m => m.InSaleId == id);
            if (salesInvoice == null)
            {
                return NotFound();
            }

            return View(salesInvoice);
        }

        // POST: SalesInvoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.SalesInvoices == null)
            {
                return Problem("Entity set 'ShowroomContext.SalesInvoices'  is null.");
            }
            var salesInvoice = await _context.SalesInvoices.FindAsync(id);
            if (salesInvoice != null)
            {
                _context.SalesInvoices.Remove(salesInvoice);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesInvoiceExists(string id)
        {
          return (_context.SalesInvoices?.Any(e => e.InSaleId == id)).GetValueOrDefault();
        }

        // GET: SalesInvoices/GetList
        [HttpGet]
        public async Task<List<SalesInvoice>> GetList(int? page, int? limits)
        {
            if (limits == null) limits = 10;
            if (page == null) page = 1;

            if (_context.SalesInvoices == null) return new List<SalesInvoice>();
            return await _context.SalesInvoices
                .Skip((page.Value - 1) * limits.Value)
                .Take(limits.Value)
                .ToListAsync();
        }
    }
}
