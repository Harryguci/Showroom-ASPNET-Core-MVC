using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShowroomManagement.Data;
using ShowroomManagement.Models;

namespace ShowroomManagement.Controllers
{
    [Authorize(Roles = "1,2")]
    public class TestDrivesController : Controller
    {
        private readonly ShowroomContext _context;
        public static int LIST_LIMITS = 5;

        public TestDrivesController(ShowroomContext context)
        {
            _context = context;
        }

        // GET: TestDrives
        public async Task<IActionResult> Index(int page = 1)
        {
            if (_context.TestDrives == null)
                return Problem("Entity set 'ShowroomContext.TestDrives'  is null.");

            var query = _context.TestDrives.Select(p => new TestDrive()
            {
                DriveId = p.DriveId,
                ClientId = p.ClientId,
                BookDate = p.BookDate,
                Note = p.Note,
                Status = p.Status,
            }).Skip(LIST_LIMITS * (page - 1))
            .Take(LIST_LIMITS).OrderByDescending(p => p.BookDate);

            var total = _context.TestDrives.Count();

            ViewBag.nextPage = true;
            ViewBag.totalRecord = total;
            ViewBag.totalPage = total / LIST_LIMITS;
            ViewBag.currentPage = page;
            return View(await query.ToListAsync());
        }

        // GET: TestDrives/Calendar
        public IActionResult Calendar()
        {
            DateTime date = DateTime.Today;
            int offset = date.DayOfWeek - DayOfWeek.Monday;
            DateTime lastMonday = date.AddDays(-offset);
            DateTime nextSunday = lastMonday.AddDays(6);

            var queryTestDrives =
                from testDrive in _context.TestDrives
                where
                    testDrive.BookDate >= lastMonday
                    && testDrive.BookDate <= nextSunday
                select testDrive;

            queryTestDrives.ToList();

            return View(queryTestDrives);
        }

        // GET: TestDrives/GetList
        public IEnumerable<TestDrive> GetList(int year, int month)
        {
            var queryTestDrives =
                from testDrive in _context.TestDrives
                where
                    testDrive.BookDate.Month == month
                    && testDrive.BookDate.Year == year
                select testDrive;

            return queryTestDrives.ToList();
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
