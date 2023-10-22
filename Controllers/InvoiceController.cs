using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ShowroomManagement.Data;
using ShowroomManagement.Models;

namespace ShowroomManagement.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ShowroomContext _context;
        private int LIST_LIMITS = 10;

        public InvoiceController(ShowroomContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (_context.PurchaseInvoices == null || _context.SalesInvoices == null) return BadRequest();
            using (var client = new HttpClient())
            {
                string absoluteUrl = @"https://localhost:3000";
                var tasks = new List<Task<HttpResponseMessage>>();

                tasks.Add(client.GetAsync(absoluteUrl + @"/SalesInvoices/GetList"));
                tasks.Add(client.GetAsync(absoluteUrl + @"/PurchaseInvoices/GetList"));

                // Parallel mutiple APIs at the same time.
                var results = await Task.WhenAll(tasks);

                var apiRes1 = await results[0].Content.ReadAsStringAsync();
                var apiRes2 = await results[1].Content.ReadAsStringAsync();

                IEnumerable<SalesInvoice>? list1 = JsonSerializer.Deserialize<List<SalesInvoice>>(apiRes1);
                IEnumerable<PurchaseInvoice>? list2 = JsonSerializer.Deserialize<List<PurchaseInvoice>>(apiRes2);

                ViewBag.PurchaseInvoices = list2;
                ViewBag.SalesInvoices = list1;
                return View();
            }
        }


    }
}
