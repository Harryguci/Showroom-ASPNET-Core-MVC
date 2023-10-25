using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShowroomManagement.Data;
using ShowroomManagement.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authentication;

namespace ShowroomManagement.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ShowroomContext _context;

        public AccountsController(ShowroomContext context)
        {
            _context = context;
        }

        // GET: Accounts
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Index()
        {
            return _context.Accounts != null ?
                        View(await _context.Accounts.ToListAsync()) :
                        Problem("Entity set 'ShowroomContext.Accounts'  is null.");
        }

        // GET: Accounts/Details/5
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Username == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,Username,Password,Level,Deleted,CreateAt,DeteleAt")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Edit/5
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "2")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AccountId,Username,Password,Level,Deleted,CreateAt,DeteleAt")] Account account)
        {
            if (id != account.Username)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Username))
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
            return View(account);
        }

        // GET: Accounts/Delete/5
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Username == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "2")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'ShowroomContext.Accounts'  is null.");
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(string id)
        {
            return (_context.Accounts?.Any(e => e.Username == id)).GetValueOrDefault();
        }

        [HttpGet]
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] Account account)
        {
            using (HttpClient client = new HttpClient())
            {
                string serialized = JsonConvert.SerializeObject(account);

                StringContent stringContent = new StringContent(serialized);
                stringContent.Headers.Remove("Content-Type");
                stringContent.Headers.Add("Content-Type", "application/json");

                //var res = await client.PostAsync(@"https://localhost:3000/api/Login", stringContent);
                var res = Authenticate(account);

                if (res != null)
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, account.Username),
                        new Claim(ClaimTypes.Role, res.Level_account.ToString()),
                        new Claim("CreateAt", res.CreateAt.ToString()),
                    };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = account.KeepLoggined
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), properties);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ValidateMessage = "User not found.";
                    return View();
                }
            }
        }

        [Authorize]
        [HttpGet("[controller]/Admins")]
        public IActionResult AdminsEndpoint()
        {
            var currentResult = GetCurrentAccount();
            if (currentResult == null)
            {
                return BadRequest("NOT FOUND current account");
            }

            return Ok($"Hi {currentResult.Username}, your level is {currentResult.Level_account}");
        }

        [HttpGet]
        [Authorize(Roles = "1, 2")]
        public Account GetCurrentAccount()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var accountClaim = identity.Claims;
                //if (accountClaim == null) return null;

                return new Account()
                {
                    Username = accountClaim.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value,
                    Level_account = Convert.ToInt32(accountClaim.FirstOrDefault(p => p.Type == ClaimTypes.Role)?.Value)
                };
            }
            return null;
        }

        private Account Authenticate(Account account)
        {
            // TODO: authenticate the account
            if (_context.Accounts == null) return null;

            var query = _context.Accounts.FromSqlRaw("SELECT * FROM DBO.LOGIN_CHECK(@username, @password)",
                 new SqlParameter("@username", account.Username),
                 new SqlParameter("@password", account.Password)).Select(p => new Account()
                 {
                     Username = p.Username,
                     Password = p.Password,
                     Level_account = p.Level_account,
                 }).FirstOrDefault();

            return query;
        }
    }
}
