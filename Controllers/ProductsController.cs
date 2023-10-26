using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShowroomManagement.Data;
using ShowroomManagement.Models;

namespace ShowroomManagement.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ShowroomContext _context;

        public ProductsController(ShowroomContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string orderby, string orderbydesc)
        {
            ViewBag.orderby = orderby;
            ViewBag.orderbydesc = orderbydesc;

            if (_context.Products == null) return BadRequest(new
            {
                error = "Can not find the table in the Database."
            });
            PropertyInfo propertyInfo = typeof(Products).GetProperty("Name");

            var query = _context.Products.Select(p => new Products()
            {
                Serial = p.Serial,
                ProductName = p.ProductName,
                PurchasePrice = p.PurchasePrice,
                SalePrice = p.SalePrice,
                Quantity = p.Quantity,
                Status = p.Status,
            });


            var list = await query.ToListAsync();

            return View(list);
        }

        // GET: Show
        public async Task<IActionResult> Show(int? page)
        {
            if (page == null) page = 1;
            int limits = 9;

            List<Products> query = await _context.Products
                .Skip((int)((page - 1) * limits))
                .Take(limits).ToListAsync();

            for (int i = 0; i < query.Count(); i++)
            {
                var imageUrls = await _context.Product_Images.Select(p => new Product_Images()
                {
                    Id = p.Id,
                    ProductSerial = p.ProductSerial,
                    Url_image = p.Url_image
                }).Where(p => p.ProductSerial == query[i].Serial)
                .ToListAsync();
                query[i].ImageUrls = imageUrls;
            }

            return View(query);
        }

        // GET: Products
        public async Task<List<Products>> Search(string q)
        {
            if (_context.Products == null) return new List<Products>();

            var query = _context.Products.Select(p => new Products()
            {
                Serial = p.Serial,
                ProductName = p.ProductName,
                PurchasePrice = p.PurchasePrice,
                SalePrice = p.SalePrice,
                Quantity = p.Quantity,
                Status = p.Status
            }).Where(p => p.ProductName.Contains(q.ToLower()));

            return await query.ToListAsync();
        }
        // GET: Products/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .FirstOrDefaultAsync(m => m.Serial == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Serial,Name,PurchasePrice,SalePrice,Quantity,SourceId,Status")] Products products)
        {
            if (ModelState.IsValid)
            {
                _context.Add(products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,
            [Bind("Serial,Name,PurchasePrice,SalePrice,Quantity,SourceId,Status")] Products products)
        {
            if (id != products.Serial)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.Serial))
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
            return View(products);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .FirstOrDefaultAsync(m => m.Serial == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ShowroomContext.Products'  is null.");
            }
            var products = await _context.Products.FindAsync(id);
            if (products != null)
            {
                _context.Products.Remove(products);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(string id)
        {
            return (_context.Products?.Any(e => e.Serial == id)).GetValueOrDefault();
        }
    }
}
