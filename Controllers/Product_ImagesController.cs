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
    public class Product_ImagesController : Controller
    {
        private readonly ShowroomContext _context;
        private int listLimits = 10;

        public Product_ImagesController(ShowroomContext context)
        {
            _context = context;
        }

        // GET: Product_Images
        public async Task<IActionResult> Index(int page = 1)
        {
            var table = _context.ProductImages;
            var list = await table.Skip((page - 1) * listLimits).Take(listLimits).ToListAsync();

            return View(list);
        }

        // GET: Product_Images/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductImages == null)
            {
                return NotFound();
            }

            var product_Images = await _context.ProductImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product_Images == null)
            {
                return NotFound();
            }

            return View(product_Images);
        }

        // GET: Product_Images/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product_Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Serial,Url_image")] ProductImages product_Images)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product_Images);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product_Images);
        }

        // GET: Product_Images/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductImages == null)
            {
                return NotFound();
            }

            var product_Images = await _context.ProductImages.FindAsync(id);
            if (product_Images == null)
            {
                return NotFound();
            }
            return View(product_Images);
        }

        // POST: Product_Images/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Serial,Url_image")] ProductImages product_Images)
        {
            if (id != product_Images.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product_Images);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Product_ImagesExists(product_Images.Id))
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
            return View(product_Images);
        }

        // GET: Product_Images/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductImages == null)
            {
                return NotFound();
            }

            var product_Images = await _context.ProductImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product_Images == null)
            {
                return NotFound();
            }

            return View(product_Images);
        }

        // POST: Product_Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductImages == null)
            {
                return Problem("Entity set 'ShowroomContext.Product_Images'  is null.");
            }
            var product_Images = await _context.ProductImages.FindAsync(id);
            if (product_Images != null)
            {
                _context.ProductImages.Remove(product_Images);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Product_ImagesExists(int id)
        {
            return _context.ProductImages.Any(e => e.Id == id);
        }
    }
}
