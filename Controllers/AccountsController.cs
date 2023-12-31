﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShowroomManagement.Data;
using ShowroomManagement.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authentication;
using System.Data;

namespace ShowroomManagement.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ShowroomContext _context;
        private int listLimits = 10;


        public AccountsController(ShowroomContext context)
        {
            _context = context;
        }

        // GET: Accounts
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Index(string asc, string desc, int page = 1)
        {
            ViewBag.asc = asc;
            ViewBag.desc = desc;

            var total = _context.Customer.Count();
            ViewBag.nextPage = true;
            ViewBag.totalRecord = total;
            ViewBag.totalPage = (int)Math.Ceiling((total - 1) * 1.0 / listLimits);
            ViewBag.currentPage = page;

            return _context.Accounts != null ?
                        View(await _context.Accounts
                        .Skip((page - 1) * listLimits)
                        .Take(listLimits)
                        .ToListAsync()) :
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Accounts");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] Account account)
        {
            string serialized = JsonConvert.SerializeObject(account);

            StringContent stringContent = new StringContent(serialized);
            stringContent.Headers.Remove("Content-Type");
            stringContent.Headers.Add("Content-Type", "application/json");

            //var res = await client.PostAsync(@"https://localhost:3000/api/Login", stringContent);
            var res = Authenticate(account);

            List<Claim> claims;

            if (res != null)
            {
                if (res.EmployeeId != null)
                {
                    claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, account.Username),
                        new Claim(ClaimTypes.Role, res.Level_account.ToString()),
                        new Claim("CreateAt", res.CreateAt.ToString()),
                        new Claim("EmployeeId", res.EmployeeId.ToString())
                    };
                }
                else
                {
                    claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, account.Username),
                        new Claim(ClaimTypes.Role, res.Level_account.ToString()),
                        new Claim("CreateAt", res.CreateAt.ToString()),
                        new Claim("CustomerId", res.ClientId.ToString())
                    };
                }

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = true
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
                    ClientId = accountClaim.FirstOrDefault(p => p.Type == "CustomerId")?.Value
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
                .FirstOrDefault();

            return query;
        }

        // GET: Accounts/Person/{id}
        [Authorize]
        public async Task<IActionResult> Person(string id = null)
        {
            Account curr = null;

            if (id == null)
            {
                curr = GetCurrentAccount();
            }
            else
            {
                curr = _context.Accounts.Where(p => p.EmployeeId == id).FirstOrDefault();
            }


            Employee currEmployee = _context.Employees
                   .Where(p => p.EmployeeId == curr.EmployeeId)
                   .FirstOrDefault();
            ViewBag.currEmployee = currEmployee;

            if (curr.Level_account == 1) // EMPLOYEE
            {
                List<SalesTarget> currSales = await _context.SalesTargets
                    .Where(p => p.EmployeeId == currEmployee.EmployeeId)
                    .ToListAsync();

                ViewBag.employeeSalesTargets = currSales;
            }
            else if (curr.Level_account == 2) // MANAGER
            {
                //TODO: Handle manager information
            }

            return View(curr);
        }

        // [Authorize]
        public async Task<IActionResult> ClientAccount()
        {
            var current = GetCurrentAccount();
            if (current.ClientId == null) return RedirectToAction("Login");
            var customer = _context.Customer
                .Where(p => p.ClientId == current.ClientId)
                .FirstOrDefault();

            var invoices = await _context.SalesInvoices
                .Where(p => p.ClientId == current.ClientId).ToListAsync();
            var products = new List<Products>();

            foreach (var invoice in invoices)
            {
                var pro = _context.Products.Find(invoice.ProductId);

                pro.ImageUrls = await _context.ProductImages
                    .Where(p => p.Serial == invoice.ProductId)
                   .Select(p => new ProductImages()
                   {
                       Id = p.Id,
                       Serial = p.Serial,
                       Url_image = p.Url_image,
                   }).ToListAsync();

                if (pro != null)
                {
                    products.Add(pro);
                }
            }
            ViewBag.products = products;
            ViewBag.invoices = invoices;
            return View(customer);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpadateAvatar(string employeeId)
        {
            var files = HttpContext.Request.Form.Files;
            foreach (var image in files)
            {
                if (image != null && image.Length > 0)
                {
                    var file = image;
                    //There is an error here
                    var uploads = Path.Combine("wwwroot", "images", "uploaded");
                    if (file.Length > 0)
                    {
                        //var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                        var fileName = file.FileName;
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                            var employee = _context.Employees.Where(p => p.EmployeeId == employeeId).FirstOrDefault();
                            employee.Url_image = "/" + Path.Combine("images", "uploaded", fileName);
                            _context.Update(employee);
                        }
                    }
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Person");
        }

        #region Sign up

        [HttpGet]
        [Authorize(Roles = "2")]
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
        [Authorize(Roles = "2")]
        public IActionResult SignUp([FromForm] string employeeHiddenId, [FromForm] string username, [FromForm] string password, [FromForm] bool isAdmin)
        {
            // Thực hiện quá trình đăng ký và kiểm tra xác thực của người dùng ở đây.
            var registrationResult = RegisterUser(username, password);

            if (registrationResult.Success)
            {
                // Nếu đăng ký thành công, thực hiện thêm dữ liệu tài khoản vào cơ sở dữ liệu bằng SQL.
                bool insertionResult = InsertAccountIntoDatabase(employeeHiddenId, username, password, isAdmin);

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


        private bool InsertAccountIntoDatabase(string employeeId, string username, string password, bool isAdmin)
        {
            // Tạo các tham số SqlParameter cho giá trị cần chèn
            //var parameter1 = new SqlParameter("@EmployeeId", value: employeeId); 
            //var parameter2 = new SqlParameter("@Username", value: username);
            //// Chuyển đổi mật khẩu (password) thành kiểu varbinary
            //var passwordBytes = Encoding.Unicode.GetBytes(password);
            //var parameter3 = new SqlParameter("@Password", SqlDbType.VarBinary, 500)
            //{
            //    Value = passwordBytes
            //};
            //var parameter4 = new SqlParameter("@CreateAt", DateTime.Now.ToString());

            ////Insert dữ liệu
            //string insertQuery = "INSERT INTO Account (EmployeeId, Username, Password_foruser, Level_account, Deleted, CreateAt) " +
            //    "VALUES ('E010', @Username, @Password , 1 , 0, @CreateAt)";


            //// Thực thi câu lệnh SQL sử dụng ExecuteSqlRaw và truyền các tham số
            //_context.Database.ExecuteSqlRaw(insertQuery, parameter1, parameter2, parameter3, parameter4);
            ////INSERT INTO Account(EmployeeId, Username, Password_foruser, Level_account, Deleted, CreateAt, DeleteAt) VALUES (N'E001', 'john_doe2', CONVERT(VARBINARY(500), 'password1'), 2, 0, GETDATE(), NULL),

            int level_account = isAdmin ? 2 : 1;

            _context.Database.ExecuteSqlRaw("INSERT INTO Account(EmployeeId, Username, Password_foruser, Level_account, Deleted, CreateAt, DeleteAt) " +
                string.Format("VALUES (N'{0}', '{1}', CONVERT(VARBINARY(500), '{2}'), {3}, 0, GETDATE(), NULL)", employeeId, username, password, level_account));

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

        private bool InsertClientAccountIntoDatabase(string customerId, string username, string password)
        {
            int level_account = 0;

            _context.Database.ExecuteSqlRaw("INSERT INTO Account(ClientId, Username, Password_foruser, Level_account, Deleted, CreateAt, DeleteAt) " +
                string.Format("VALUES ('{0}', '{1}', CONVERT(VARBINARY(500), '{2}'), {3}, 0, GETDATE(), NULL)", customerId, username, password, level_account));

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


        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignUpClient()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUpClient(string Firstname, string Lastname,
            string Cccd, bool Gender, string DateBirth,
            string username, string password)
        {
            if (username == null || password == null || Firstname == null || Lastname == null
                || Firstname == string.Empty || Lastname == string.Empty || DateBirth == null)
            {
                return View();
            }
            var id = _context.Customer.Select(p => p.ClientId).OrderByDescending(p => p).FirstOrDefault().Substring(1);
            id = (Convert.ToInt32(id) + 1).ToString();

            for (int i = 0; i < 3 - id.ToString().Length; i++)
            {
                id = "0" + id;
            }
            id = "C" + id;

            Customer customer = new Customer()
            {
                ClientId = id,
                Firstname = Firstname,
                Lastname = Lastname,
                Cccd = Cccd,
                Gender = Gender,
                DateBirth = DateTime.Parse(DateBirth),
            };

            _context.Add(customer);
            await _context.SaveChangesAsync();

            InsertClientAccountIntoDatabase(customer.ClientId, username, password);

            return RedirectToAction("Login");
        }
        #endregion
    }

    #region Tạo class RegistrationResult (vì ko biết đặt đâu)
    public class RegistrationResult
    {
        public RegistrationResult()
        {
            Success = true;
            ErrorMessage = string.Empty;
        }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
    #endregion
}
