using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShowroomManagement.Data;
using ShowroomManagement.Models;
using System.Security.Claims;

namespace ShowroomManagement.Controllers
{
    public class ClientHomeController : Controller
    {
        private readonly ShowroomContext _context;

        public ClientHomeController(ShowroomContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
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
                    Level_account = Convert.ToInt32(accountClaim.FirstOrDefault(p => p.Type == ClaimTypes.Role)?.Value),
                    EmployeeId = accountClaim.FirstOrDefault(p => p.Type == "EmployeeId")?.Value,
                    CustomerId = accountClaim.FirstOrDefault(p => p.Type == "CustomerId")?.Value
                };
            }
            return null;
        }

        [AllowAnonymous]
        public IActionResult Booking()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Booking([Bind("BookDate, Note")] TestDrive testdrive)
        {
            // Handle booking
            var currentAccount = GetCurrentAccount();
            var id = _context.TestDrives
                .Select(p => new TestDrive() { DriveId = p.DriveId})
                .OrderByDescending(p => p.DriveId)
                .FirstOrDefault().DriveId;

            var idNum = Convert.ToInt32(id.Substring(2)) + 1;
            id = idNum.ToString();
            for (int i = 1; i <= 3 - idNum.ToString().Length; i++)
            {
                id = "0" + id;
            }
            id = "TD" + id;
            testdrive.DriveId = id;
            testdrive.ClientId = currentAccount.CustomerId;
            testdrive.EmployeeId = null;

            _context.Add(testdrive);
            await _context.SaveChangesAsync();

            return View();
        }
    }
}
