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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;
using System.Text;

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
        [Authorize(Roles = "2")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        [HttpPost]
        [Authorize(Roles = "2")]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
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
                        new Claim("EmployeeId", res.EmployeeId.ToString())
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
                    EmployeeId = accountClaim.FirstOrDefault(p => p.Type == "EmployeeId")?.Value
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
                 new SqlParameter("@password", account.Password))
                .Select(p => new Account()
                {
                    Username = p.Username,
                    Password = p.Password,
                    EmployeeId = p.EmployeeId,
                    Level_account = p.Level_account,
                }).FirstOrDefault();

            return query;
        }

        // GET: Accounts/Person/{id}
        [Authorize]
        public async Task<IActionResult> Person(string id = null)
        {
            if (id == null)
            {
                var curr = GetCurrentAccount();

                Employee currEmployee = _context.Employees
                       .Where(p => p.EmployeeId == curr.EmployeeId)
                       .FirstOrDefault();
                ViewBag.currEmployee = currEmployee;

                if (curr.Level_account == 1) // EMPLOYEE
                {
                    List<SalesTarget> currSales = await _context.SalesTargets
                        .Where(p => p.SaleId == currEmployee.SaleId)
                        .ToListAsync();

                    ViewBag.employeeSalesTargets = currSales;
                   
                } else if (curr.Level_account == 2) // MANAGER
                {
                    //TODO: Handle manager information
                }

                return View(curr);
            }
            else
            {
                var account = _context.Accounts.Find(id);
                return View(account);
            }
        }

        #region Sign up

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignUp()
        {
            if (_context.Employees == null) return BadRequest();
            var employees = _context.Employees
            .Where(p => !p.Deleted)
            .Select(p => new Employee
            {
                EmployeeId = p.EmployeeId,
                Firstname = p.Firstname,
                Lastname = p.Lastname,
            })
            .ToList();

            return View(employees);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromForm] string employeeHiddenId, [FromForm] string username, [FromForm] string password)
        {
            using (HttpClient client = new HttpClient())
            {
                // Thực hiện quá trình đăng ký và kiểm tra xác thực của người dùng ở đây.
                var registrationResult = RegisterUser(username, password);

                if (registrationResult.Success)
                {
                    // Nếu đăng ký thành công, thực hiện thêm dữ liệu tài khoản vào cơ sở dữ liệu bằng SQL.
                    bool insertionResult = InsertAccountIntoDatabase(employeeHiddenId, username, password);

                    if (insertionResult)
                    {
                        return RedirectToAction("Login", "Accounts");
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    // Nếu đăng ký không thành công, hiển thị thông báo lỗi và giữ người dùng ở trang đăng ký.
                    ViewBag.ValidateMessage = registrationResult.ErrorMessage;
                    return View();
                }
            }
        }


        private bool InsertAccountIntoDatabase(string employeeId, string username, string password)
        {   
            // Tạo các tham số SqlParameter cho giá trị cần chèn
            var parameter1 = new SqlParameter("@EmployeeId", employeeId);
            var parameter2 = new SqlParameter("@Username", username);
            // Chuyển đổi mật khẩu (password) thành kiểu varbinary
            var passwordBytes = Encoding.Unicode.GetBytes(password);
            var parameter3 = new SqlParameter("@Password", SqlDbType.VarBinary, passwordBytes.Length)
            {
                Value = passwordBytes
            };
            var parameter4 = new SqlParameter("@CreateAt", Convert.ToString(DateTime.Now));

            //Insert dữ liệu
            string insertQuery = "INSERT INTO Account (EmployeeId, Username, Password_foruser, Level_account, Deleted, CreateAt) " +
                "VALUES ('E010', @Username, @Password , 1 , 0, @CreateAt)";
            
            
            // Thực thi câu lệnh SQL sử dụng ExecuteSqlRaw và truyền các tham số
            _context.Database.ExecuteSqlRaw(insertQuery, parameter1, parameter2, parameter3, parameter4);

            //Kiểm tra 
            bool Exist = _context.Accounts.Any(a => a.Username == username);


            if (Exist)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public RegistrationResult RegisterUser(string username, string password)
        {
            // Kiểm tra xem tên đăng nhập đã tồn tại trong cơ sở dữ liệu chưa
            string checkUsernameQuery = "SELECT COUNT(*) FROM Account WHERE Username = @Username";
            var usernameParameter = new SqlParameter("@Username", username);
            int existingUserCount = _context.Database.ExecuteSqlRaw(checkUsernameQuery, usernameParameter);

            if (existingUserCount > 0)
            {
                return new RegistrationResult
                {
                    Success = false,
                    ErrorMessage = "Tên đăng nhập đã tồn tại. Vui lòng chọn tên đăng nhập khác."
                };
            }

            // Tiếp tục kiểm tra các điều kiện khác và thực hiện lưu thông tin người dùng vào cơ sở dữ liệu (nếu hợp lệ)

            return new RegistrationResult
            {
                Success = true
            };
        }


        #endregion
    }

    #region Tạo class RegistrationResult (vì ko biết đặt đâu)
    public class RegistrationResult
    {   
        public RegistrationResult ()
        {
            Success = true;
            ErrorMessage = string.Empty;
        }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
    #endregion
}
