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
    public class PurchaseInvoicesController : Controller
    {
        private readonly ShowroomContext _context;

        public PurchaseInvoicesController(ShowroomContext context)
        {
            _context = context;
        }

        // GET: PurchaseInvoices
        public async Task<IActionResult> Index()
        {
            return _context.PurchaseInvoices != null ?
                        View(await _context.PurchaseInvoices.ToListAsync()) :
                        Problem("Entity set 'ShowroomContext.PurchaseInvoices'  is null.");
        }

        // GET: PurchaseInvoices/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.PurchaseInvoices == null)
            {
                return NotFound();
            }

            var purchaseInvoice = await _context.PurchaseInvoices
                .FirstOrDefaultAsync(m => m.InEnterId == id);
            if (purchaseInvoice == null)
            {
                return NotFound();
            }

            return View(purchaseInvoice);
        }

        // GET: PurchaseInvoices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PurchaseInvoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InEnterId,Source,EnteredDate,QuantityPurchase,Status")] PurchaseInvoice purchaseInvoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchaseInvoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(purchaseInvoice);
        }

        // GET: PurchaseInvoices/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.PurchaseInvoices == null)
            {
                return NotFound();
            }

            var purchaseInvoice = await _context.PurchaseInvoices.FindAsync(id);
            if (purchaseInvoice == null)
            {
                return NotFound();
            }
            return View(purchaseInvoice);
        }

        // POST: PurchaseInvoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("InEnterId,Source,EnteredDate,QuantityPurchase,Status")] PurchaseInvoice purchaseInvoice)
        {
            if (id != purchaseInvoice.InEnterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchaseInvoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseInvoiceExists(purchaseInvoice.InEnterId))
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
            return View(purchaseInvoice);
        }

        // GET: PurchaseInvoices/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.PurchaseInvoices == null)
            {
                return NotFound();
            }

            var purchaseInvoice = await _context.PurchaseInvoices
                .FirstOrDefaultAsync(m => m.InEnterId == id);
            if (purchaseInvoice == null)
            {
                return NotFound();
            }

            return View(purchaseInvoice);
        }

        // POST: PurchaseInvoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.PurchaseInvoices == null)
            {
                return Problem("Entity set 'ShowroomContext.PurchaseInvoices'  is null.");
            }
            var purchaseInvoice = await _context.PurchaseInvoices.FindAsync(id);
            if (purchaseInvoice != null)
            {
                _context.PurchaseInvoices.Remove(purchaseInvoice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseInvoiceExists(string id)
        {
            return (_context.PurchaseInvoices?.Any(e => e.InEnterId == id)).GetValueOrDefault();
        }


        // GET: PurchaseInvoices/GetList
        [HttpGet]
        public async Task<List<PurchaseInvoice>> GetList(int? page, int? limits)
        {
            if (limits == null) limits = 10;
            if (page == null) page = 1;
            if (_context.PurchaseInvoices == null) return new List<PurchaseInvoice>();

            return await _context.PurchaseInvoices
                .Skip((page - 1).Value * limits.Value)
                .Take(limits.Value)
                .ToListAsync();
        }
    }
}
