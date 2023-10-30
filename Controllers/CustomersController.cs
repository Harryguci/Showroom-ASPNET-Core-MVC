using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using ShowroomManagement.Data;
using ShowroomManagement.Models;

namespace ShowroomManagement.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ShowroomContext _context;
        private int listLimits = 10;

        public CustomersController(ShowroomContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "1, 2")]
        // GET: Customers
        public async Task<IActionResult> Index(string asc, string desc, int page = 1)
        {
            ViewBag.asc = asc;
            ViewBag.desc = desc;

            var total = _context.Customer.Count();
            ViewBag.nextPage = true;
            ViewBag.totalRecord = total;
            ViewBag.totalPage = (int)Math.Ceiling((total - 1) * 1.0 / listLimits);
            ViewBag.currentPage = page;

            if (asc != null)
            {
                switch (asc.ToLower())
                {
                    case "clientname":
                        return _context.Customer != null ?
                        View(await _context.Customer.OrderBy(p => p.ClientId).Skip((page - 1) * listLimits).Take(listLimits).ToListAsync()) :
                        Problem("Entity set 'ShowroomContext.Customer'  is null.");
                    case "firstname":
                        return _context.Customer != null ?
                        View(await _context.Customer.OrderBy(p => p.Firstname).ToListAsync()) :
                        Problem("Entity set 'ShowroomContext.Customer'  is null.");
                    case "lastname":
                        return _context.Customer != null ?
                        View(await _context.Customer.OrderBy(p => p.Lastname).ToListAsync()) :
                        Problem("Entity set 'ShowroomContext.Customer'  is null.");
                    case "datebirth":
                        return _context.Customer != null ?
                        View(await _context.Customer.OrderBy(p => p.DateBirth).ToListAsync()) :
                        Problem("Entity set 'ShowroomContext.Customer'  is null.");
                    case "cccd":
                        return _context.Customer != null ?
                        View(await _context.Customer.OrderBy(p => p.Cccd).ToListAsync()) :
                        Problem("Entity set 'ShowroomContext.Customer'  is null.");
                    case "email":
                        return _context.Customer != null ?
                        View(await _context.Customer.OrderBy(p => p.Email).ToListAsync()) :
                        Problem("Entity set 'ShowroomContext.Customer'  is null.");
                    case "gender":
                        return _context.Customer != null ?
                        View(await _context.Customer.OrderBy(p => p.Gender).ToListAsync()) :
                        Problem("Entity set 'ShowroomContext.Customer'  is null.");
                    default:
                        return _context.Customer != null ?
                        View(await _context.Customer.OrderBy(p => p.ClientId).ToListAsync()) :
                        Problem("Entity set 'ShowroomContext.Customer'  is null.");
                }
            }
            else if (desc != null)
            {
                switch (desc.ToLower())
                {
                    case "clientname":
                        return _context.Customer != null ?
                        View(await _context.Customer.OrderByDescending(p => p.ClientId).ToListAsync()) :
                        Problem("Entity set 'ShowroomContext.Customer'  is null.");
                    case "firstname":
                        return _context.Customer != null ?
                        View(await _context.Customer.OrderByDescending(p => p.Firstname).ToListAsync()) :
                        Problem("Entity set 'ShowroomContext.Customer'  is null.");
                    case "lastname":
                        return _context.Customer != null ?
                        View(await _context.Customer.OrderByDescending(p => p.Lastname).ToListAsync()) :
                        Problem("Entity set 'ShowroomContext.Customer'  is null.");
                    case "datebirth":
                        return _context.Customer != null ?
                        View(await _context.Customer.OrderByDescending(p => p.DateBirth).ToListAsync()) :
                        Problem("Entity set 'ShowroomContext.Customer'  is null.");
                    case "cccd":
                        return _context.Customer != null ?
                        View(await _context.Customer.OrderByDescending(p => p.Cccd).ToListAsync()) :
                        Problem("Entity set 'ShowroomContext.Customer'  is null.");
                    case "email":
                        return _context.Customer != null ?
                        View(await _context.Customer.OrderByDescending(p => p.Email).ToListAsync()) :
                        Problem("Entity set 'ShowroomContext.Customer'  is null.");
                    case "gender":
                        return _context.Customer != null ?
                        View(await _context.Customer.OrderByDescending(p => p.Gender).ToListAsync()) :
                        Problem("Entity set 'ShowroomContext.Customer'  is null.");
                    default:
                        return _context.Customer != null ?
                        View(await _context.Customer.OrderByDescending(p => p.ClientId).ToListAsync()) :
                        Problem("Entity set 'ShowroomContext.Customer'  is null.");
                }
            }

            return _context.Customer != null ?
                        View(await _context.Customer.ToListAsync()) :
                        Problem("Entity set 'ShowroomContext.Customer'  is null.");
        }

        // GET: Customers/Search
        [Authorize(Roles = "1, 2")]
        public async Task<List<Customer>> Search(string q)
        {
            if (_context.Customer == null) return new List<Customer>();

            var query = _context.Customer.Where(p => !p.Deleted
                && (p.ClientId.ToLower().Contains(q)
                || p.Firstname.ToLower().Contains(q)
                || p.Lastname.ToLower().Contains(q))
            );

            return await query.ToListAsync();
        }

        // GET: Customers/Details/5
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Customer == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.ClientId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        [Authorize(Roles = "1, 2")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> Create([Bind("Firstname,Lastname,DateBirth,Cccd,Email,Address,Gender")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                var id = Convert.ToInt32(_context.Customer
                    .OrderByDescending(p => p.ClientId)
                    .FirstOrDefault().ClientId.Substring(1)) + 1;

                customer.ClientId = "C" + Convert.ToInt32(id);
                _context.Add(customer);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Customer == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> Edit(string id, [Bind("ClientId,Firstname,Lastname,DateBirth,Cccd,Email,Address,Gender")] Customer customer)
        {
            if (id != customer.ClientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.ClientId))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Customer == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.ClientId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Customer == null)
            {
                return Problem("Entity set 'ShowroomContext.Customer'  is null.");
            }
            var customer = await _context.Customer.FindAsync(id);
            if (customer != null)
            {
                _context.Customer.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "1, 2")]
        private bool CustomerExists(string id)
        {
            return (_context.Customer?.Any(e => e.ClientId == id)).GetValueOrDefault();
        }
    }
}
