﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShowroomManagement.Data;
using ShowroomManagement.Models;

namespace ShowroomManagement.Controllers
{
    [Authorize]
    public class SalesInvoicesController : Controller
    {
        private readonly ShowroomContext _context;
        private int listLimits = 10;

        public SalesInvoicesController(ShowroomContext context)
        {
            _context = context;
        }

        // GET: SalesInvoices
        public async Task<IActionResult> Index(int page = 1)
        {
            if (_context.SalesInvoices == null)
                return Problem("Entity set 'ShowroomContext.SalesInvoices'  is null.");

            var query = await _context.SalesInvoices
                .Skip((page - 1) * listLimits)
                .Take(listLimits)
                .ToListAsync();

            var total = _context.SalesInvoices.Count();

            ViewBag.nextPage = true;
            ViewBag.totalRecord = total;
            ViewBag.totalPage = (int)Math.Ceiling(total * 1.0 / listLimits);
            ViewBag.currentPage = page;

            return View(query);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId,SaleDate,ProductId,Status,QuantitySale")] SalesInvoice salesInvoice)
        {
            if (ModelState.IsValid)
            {
                var id = _context.SalesInvoices.Select(p => p.InSaleId).OrderByDescending(p => p).FirstOrDefault();
                id = id.Substring(2);
                id = (Convert.ToInt32(id) + 1).ToString();

                for (int i = 0; i < 3 - Convert.ToInt32(id).ToString().Length; i++)
                {
                    id = "0" + id;
                }

                id = "SI" + id;
                salesInvoice.InSaleId = id;

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("InSaleId,ClientId,ProductId,SaleDate,Status,QuantitySale")] SalesInvoice salesInvoice)
        {
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
                return NotFound();

            var salesInvoice = await _context.SalesInvoices
                .FirstOrDefaultAsync(m => m.InSaleId == id);

            if (salesInvoice == null)
                return NotFound();

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

            if (_context.SalesInvoices == null)
                return new List<SalesInvoice>();

            return await _context.SalesInvoices
                .Skip((page.Value - 1) * limits.Value)
                .Take(limits.Value)
                .Skip((page.Value - 1) * listLimits).Take(listLimits).ToListAsync();
        }
    }
}
