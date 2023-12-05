using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ShowroomManagement.Data;
using ShowroomManagement.Models;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Data;

//using Newtonsoft.Json;

namespace ShowroomManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ShowroomContext _context;

        public HomeController(ILogger<HomeController> logger, ShowroomContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = GetCurrentAccount();
            if (user.Level_account >= 1)
            {
                var employeeNum = _context.Employees.Where(p => p.Position.ToLower() == "sales").Count();
                var managerNum = _context.Employees.Where(p => p.Position.ToLower() == "sales manager").Count();
                var customerNum = _context.Customer.Count();

                var listLimits = 10;

                var employees = await _context.Employees.Take(listLimits)
                    .OrderByDescending(p => p.EmployeeId).ToListAsync();

                var products = await _context.Products.Take(listLimits)
                    .OrderByDescending(p => p.Serial).ToListAsync();

                var customers = await _context.Customer.Take(listLimits)
                    .OrderByDescending(p => p.ClientId).ToListAsync();

                ViewBag.customers = customers;
                ViewBag.employees = employees;
                ViewBag.products = products;

                ViewBag.employeeNum = employeeNum;
                ViewBag.managerNum = managerNum;
                ViewBag.customerNum = customerNum;

                return View();
            }
            return Redirect("/ClientHome/Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET: Home/Search
        [HttpGet]
        [Authorize]
        public IActionResult Search(string q)
        {
            ViewBag.q = q;

            q = q.ToLower();

            var customersResponse = _context.Customer.Take(10)
                    .Where(p => (p.Firstname.ToLower().Contains(q) || p.Lastname.ToLower().Contains(q))).ToJson();

            var productsResponse = _context.Products.Take(10)
                .Where(p => (p.Serial.ToLower().Contains(q) || p.ProductName.ToLower().Contains(q))).ToJson();

            var employeesResponse = _context.Employees.Take(10)
                .Where(p => (p.Firstname.ToLower().Contains(q) || p.Lastname.ToLower().Contains(q))).ToJson();

            customersResponse = customersResponse != null ? customersResponse : "[]";
            productsResponse = productsResponse != null ? productsResponse : "[]";
            employeesResponse = employeesResponse != null ? employeesResponse : "[]";

            try
            {
                IEnumerable<Customer> customers = JsonSerializer.Deserialize<List<Customer>>(customersResponse);
                IEnumerable<Products> products = JsonSerializer.Deserialize<List<Products>>(productsResponse);
                IEnumerable<Employee> employees = JsonSerializer.Deserialize<List<Employee>>(employeesResponse);
                ViewBag.customers = customers;
                ViewBag.products = products;
                ViewBag.employees = employees;

                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "1,2")]
        public string SearchApi(string q)
        {
            if (q == null) return "[]";
            q = q.ToLower();

            var customersResponse = _context.Customer.Take(10)
                    .Where(p => (p.Firstname.ToLower().Contains(q) || p.Lastname.ToLower().Contains(q))).ToJson();

            var productsResponse = _context.Products.Take(10)
                .Where(p => (p.Serial.ToLower().Contains(q) || p.ProductName.ToLower().Contains(q))).ToJson();

            var employeesResponse = _context.Employees.Take(10)
                .Where(p => (p.Firstname.ToLower().Contains(q) || p.Lastname.ToLower().Contains(q))).ToJson();

            customersResponse = customersResponse != null ? customersResponse : "[]";
            productsResponse = productsResponse != null ? productsResponse : "[]";
            employeesResponse = employeesResponse != null ? employeesResponse : "[]";

            try
            {
                IEnumerable<Customer> customers = JsonSerializer.Deserialize<List<Customer>>(customersResponse);
                IEnumerable<Products> products = JsonSerializer.Deserialize<List<Products>>(productsResponse);
                IEnumerable<Employee> employees = JsonSerializer.Deserialize<List<Employee>>(employeesResponse);

                return JsonSerializer.Serialize(new
                {
                    customers = customers,
                    products = products,
                    employees = employees
                });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new { error = ex.Message });
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


        [HttpGet]
        [Authorize(Roles = "2")]
        public async Task<string> GetToTalQuantity(string year)
        {
            var result = new List<int>();
            for (var month = 1; month <= 12; month++)
            {
                var query = await _context.SalesInvoices
                    .Where(p => p.SaleDate.Month == month)
                    .GroupBy(p => 1)
                    .Select(p => new { total = p.Sum(e => e.QuantitySale) })
                    .ToListAsync();

                if (query != null && query.Count > 0)
                    result.Add(query[0].total);
                else result.Add(0);
            }

            return JsonSerializer.Serialize(result);
        }

        [HttpGet]
        [Authorize(Roles = "2")]
        public async Task<string> GetToTalQuantityEachYear(string year)
        {
            var result = new List<int>();

            for (int y = DateTime.Now.Year; y >= DateTime.Now.Year - 5; y--)
            {
                var query = await _context.SalesInvoices
                    .Where(p => p.SaleDate.Year == y)
                    .GroupBy(p => 1)
                    .Select(p => new { total = p.Sum(e => e.QuantitySale) })
                    .ToListAsync();

                if (query != null && query.Count > 0)
                    result.Add(query[0].total);
                else result.Add(0);
            }

            result.Reverse();

            return JsonSerializer.Serialize(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Accounts");
        }
    }
}