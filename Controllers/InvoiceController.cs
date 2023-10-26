using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ShowroomManagement.Data;
using ShowroomManagement.Models;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol;

namespace ShowroomManagement.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        private readonly ShowroomContext _context;
        private int LIST_LIMITS = 10;

        public InvoiceController(ShowroomContext context)
        {
            _context = context;
        }

        // GET: Invoices
        [HttpGet]
        [Authorize(Roles = "1,2")]
        public IActionResult Index()
        {
            if (_context.PurchaseInvoices == null || _context.SalesInvoices == null) return BadRequest();
            using (var client = new HttpClient())
            {
                var apiRes1 = _context.SalesInvoices.OrderByDescending(p => p.SaleDate).Take(LIST_LIMITS).ToJson();
                var apiRes2 = _context.PurchaseInvoices.OrderByDescending(p => p.EnteredDate).Take(LIST_LIMITS).ToJson();

                IEnumerable<SalesInvoice> salesInvoices = JsonSerializer.Deserialize<List<SalesInvoice>>(apiRes1);
                IEnumerable<PurchaseInvoice> purchaseInvoices = JsonSerializer.Deserialize<List<PurchaseInvoice>>(apiRes2);

                ViewBag.PurchaseInvoices = purchaseInvoices;
                ViewBag.SalesInvoices = salesInvoices;

                return View();
            }
        }
    }
}
