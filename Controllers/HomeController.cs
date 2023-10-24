using Microsoft.AspNetCore.Mvc;
using ShowroomManagement.Data;
using ShowroomManagement.Models;
using System.Diagnostics;
using System.Text.Json;

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
        public async Task<IActionResult> Search(string q)
        {
            ViewBag.q = q;

            using (HttpClient client = new HttpClient())
            {
                string absoluteUrl = @"https://localhost:3000";

                var getEmployees = client.GetAsync(absoluteUrl + @"/Employees/Search?q=" + q);
                var getProducts = client.GetAsync(absoluteUrl + @"/Products/Search?q=" + q);
                var getCustomers = client.GetAsync(absoluteUrl + @"/Customers/Search?q=" + q);

                List<Task<HttpResponseMessage>> tasks = new List<Task<HttpResponseMessage>>();
                tasks.Add(getCustomers);
                tasks.Add(getProducts);
                tasks.Add(getEmployees);

                var responses = await Task.WhenAll(tasks);

                var customersResponse = await responses[0].Content.ReadAsStringAsync();
                var productsResponse = await responses[1].Content.ReadAsStringAsync();
                var employeesResponse = await responses[2].Content.ReadAsStringAsync();

                customersResponse = customersResponse != null ? customersResponse : "[]";
                productsResponse = productsResponse != null ? productsResponse : "[]";
                employeesResponse = employeesResponse != null ? employeesResponse : "[]";

                try
                {
                    IEnumerable<Customer>? customers = JsonSerializer.Deserialize<List<Customer>>(customersResponse);
                    IEnumerable<Products>? products = JsonSerializer.Deserialize<List<Products>>(productsResponse);
                    IEnumerable<Employee>? employees = JsonSerializer.Deserialize<List<Employee>>(employeesResponse);
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
    }
}