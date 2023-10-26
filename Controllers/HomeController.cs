using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ShowroomManagement.Data;
using ShowroomManagement.Models;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol;

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

        public IActionResult Index()
        {
            return View();
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
        public IActionResult Search(string q)
        {
            ViewBag.q = q;
            q = q.ToLower();

            using (HttpClient client = new HttpClient())
            {
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
        }

        public static async Task<Account> GetCurrentAccount()
        {
            using (HttpClient client = new HttpClient())
            {
                var currentAcc = await client.GetAsync("https://localhost:3000/Accounts/GetCurrentAccount");
                return JsonSerializer.Deserialize<Account>(await currentAcc.Content.ReadAsStringAsync());
            }
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