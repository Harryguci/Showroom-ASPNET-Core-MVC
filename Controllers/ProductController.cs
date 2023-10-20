using Microsoft.AspNetCore.Mvc;
using ShowroomManagement.Data;
using ShowroomManagement.Models;

namespace ShowroomManagement.Controllers
{
    public class ProductController : Controller
    {
        private ShowroomContext db;

        public ProductController(ShowroomContext context) { 
            db = context;
        }

        public IActionResult Index()
        {
            List<Products>? products = db.Products != null ? db.Products.ToList() : null;

            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create([Bind("Serial,Name,Value,InEnterId,InSaleId,Status")] Products product)
        {
            return Ok(product);
        }
    }
}
