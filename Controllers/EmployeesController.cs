using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShowroomManagement.Data;
using ShowroomManagement.Models;

namespace ShowroomManagement.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ShowroomContext _context;
        private static int listLimits = 10;

        public EmployeesController(ShowroomContext context)
        {
            _context = context;
        }

        // GET: Employees
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Index(int? page = 1)
        {
            if (_context.Employees == null) return BadRequest();
            var skip = (page - 1) * listLimits;
            skip = skip != null ? skip : 0;

            var query = _context.Employees.Select(p => new Employee()
            {
                EmployeeId = p.EmployeeId,
                Firstname = p.Firstname,
                Lastname = p.Lastname,
                DateBirth = p.DateBirth,
                Cccd = p.Cccd,
                Position = p.Position,
                StartDate = p.StartDate,
                Salary = p.Salary,
                Email = p.Email,
                Gender = p.Gender,
                Deleted = p.Deleted,
            })
            .Where(p => !p.Deleted)
            .Skip((int)skip).Take(listLimits);

            var total = _context.Employees.Count();

            ViewBag.nextPage = true;
            ViewBag.totalRecord = total;
            ViewBag.totalPage = (int)Math.Ceiling((total - 1) * 1.0 / listLimits);
            ViewBag.currentPage = page;
            ViewBag.TrashTotal = _context.Employees.Select(p => p.Deleted).Where(p => p).Count();

            return View(await query.Skip((page.Value - 1) * listLimits).Take(listLimits).ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            var employeeAcc = _context.Accounts.Where(p => p.EmployeeId == id).FirstOrDefault();
            ViewBag.employeeAccount = employeeAcc;

            return View(employee);
        }

        // GET: Employees/Create
        [Authorize(Roles = "2")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Create([Bind("Firstname,Lastname,DateBirth,Cccd,Position,StartDate,Salary,Email,Gender")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var id = Convert.ToInt32(_context.Employees.OrderByDescending(p => p.EmployeeId).FirstOrDefault().EmployeeId.Substring(1)) + 1;
                var idStr = id.ToString();
                for (int i = 1; i <= 3 - id.ToString().Length; i++)
                {
                    idStr = "0" + idStr;
                }

                idStr = "E" + idStr;
                employee.EmployeeId = idStr;



                // create a new SaleTarget

                var stId = (Convert.ToInt32(_context.SalesTargets.Select(p => p.SaleId)
                    .OrderByDescending(p => p).FirstOrDefault().Substring(1)) + 1).ToString();

                for (int i = 1; i < 3 - stId.Length; i++)
                {
                    stId = "0" + stId;
                }

                stId = "S" + stId;

                SalesTarget target = new SalesTarget()
                {
                    SaleId = stId,
                    EmployeeId = employee.EmployeeId,
                    StartDate = DateTime.Now,
                    EndDate = new DateTime(DateTime.Now.Year,
                                DateTime.Now.Month,
                                DateTime.DaysInMonth(DateTime.Now.Year,
                                DateTime.Now.Month)),
                    Total = 0,
                    Target = 10,
                };

                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
                ViewBag.errors = errors;
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Edit(string id, [Bind("EmployeeId,Firstname,Lastname,DateBirth,Cccd,Postion,StartDate,Salary,Email,SaleId,Gender")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
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
            return View(employee);
        }

        [HttpGet]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Trash(int? page, int? limits)
        {
            if (_context.Employees == null) return BadRequest();
            if (page == null) page = 1;
            if (limits == null) limits = 10;

            var query = _context.Employees.Select(p => new Employee()
            {
                EmployeeId = p.EmployeeId,
                Firstname = p.Firstname,
                Lastname = p.Lastname,
                DateBirth = p.DateBirth,
                Cccd = p.Cccd,
                Position = p.Position,
                StartDate = p.StartDate,
                Salary = p.Salary,
                Email = p.Email,
                Gender = p.Gender,
                Deleted = p.Deleted,
            }).Where(p => p.Deleted)
            .Skip((page - 1).Value * limits.Value)
            .Take(limits.Value);

            return View(await query.Skip((page.Value - 1) * listLimits).Take(listLimits).ToListAsync());
        }

        // GET: Employees/Delete/5
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost]
        [Authorize(Roles = "2")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'ShowroomContext.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            var testDrives = await _context.TestDrives.Where(p => p.EmployeeId == id).ToListAsync();
            var accounts = await _context.Accounts.Where(p => p.EmployeeId == id).ToListAsync();

            if (accounts != null && accounts.Count > 0)
            {
                for (int i = 0; i < accounts.Count; i++)
                {
                    _context.Accounts.Remove(accounts[i]);
                }
            }

            await _context.SaveChangesAsync();

            if (testDrives != null && testDrives.Count > 0)
            {
                for (int i = 0; i < testDrives.Count; i++)
                {
                    _context.TestDrives.Remove(testDrives[i]);
                }
            }

            await _context.SaveChangesAsync();

            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Employees/DeleteSoft
        [HttpPost]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> DeleteSoft(string id)
        {
            // TODO: Move the employee which has
            // EmployeeId equals id to Bin trash (Set Deteted Prop to TRUE).

            if (_context.Employees == null)
            {
                return Problem("Entity set 'ShowroomContext.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                employee.Deleted = true;
                _context.Employees.Update(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "2")]
        public IActionResult UndoFromTrash(string id)
        {
            var query = _context.Employees.Find(id);

            if (query == null)
            {
                return BadRequest("Can not find the employee");
            }

            query.Deleted = false;

            _context.Employees.Update(query);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(string id)
        {
            return (_context.Employees?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
        }

        [HttpGet]
        [Authorize(Roles = "1")]
        public IActionResult SignWorkDate()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public IActionResult SignWorkDate(string workdate, string calam)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var accountClaim = identity.Claims;

                var currentAccount = new Account()
                {
                    Username = accountClaim.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value,
                    Level_account = Convert.ToInt32(accountClaim.FirstOrDefault(p => p.Type == ClaimTypes.Role)?.Value),
                    EmployeeId = accountClaim.FirstOrDefault(p => p.Type == "EmployeeId")?.Value
                };

                return Ok(new
                {
                    user = currentAccount,
                    sign = new { workdate = workdate, calam = calam }
                });
            }

            return View();
        }

        // GET: Employees/Search
        [HttpGet]
        [Authorize(Roles = "2")]
        public async Task<List<Employee>> Search(string q)
        {
            if (_context.Employees == null)
                return new List<Employee>();
            q = q.ToLower().Trim();

            var query = _context.Employees.Select(p => new Employee()
            {
                EmployeeId = p.EmployeeId,
                Firstname = p.Firstname,
                Lastname = p.Lastname,
                DateBirth = p.DateBirth,
                Cccd = p.Cccd,
                Position = p.Position,
                StartDate = p.StartDate,
                Salary = p.Salary,
                Email = p.Email,
                Gender = p.Gender,
                Deleted = p.Deleted,
            }).Where(p => !p.Deleted && (p.Firstname.ToLower().Contains(q)
            || (p.Lastname.ToLower().Contains(q))
            || (p.EmployeeId.ToLower().Contains(q))));

            return await query.ToListAsync();
        }
    }
}
