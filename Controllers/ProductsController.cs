using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShowroomManagement.Data;
using ShowroomManagement.Models;

namespace ShowroomManagement.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ShowroomContext _context;
        private int listLimits = 10;

        public ProductsController(ShowroomContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string orderby, string orderbydesc, int page = 1)
        {
            ViewBag.orderby = orderby;
            ViewBag.orderbydesc = orderbydesc;
            ViewBag.trashNumber = _context.Products.Where(p => p.Deleted.Value).Count();

            if (_context.Products == null) return BadRequest(new
            {
                error = "Can not find the table in the Database."
            });

            PropertyInfo propertyInfo = typeof(Products).GetProperty("Name");

            var query = await _context.Products.Select(p => new Products()
            {
                Serial = p.Serial,
                ProductName = p.ProductName,
                PurchasePrice = p.PurchasePrice,
                SalePrice = p.SalePrice,
                Quantity = p.Quantity,
                Status = p.Status,
                Deleted = p.Deleted
            })
            .Where(p => !(bool)p.Deleted)
            .Skip((page - 1) * listLimits)
            .Take(listLimits).ToListAsync();

            var total = _context.Products.Count();

            ViewBag.nextPage = true;
            ViewBag.totalRecord = total;
            ViewBag.totalPage = (int)Math.Ceiling(total * 1.0 / listLimits);
            ViewBag.currentPage = page;

            return View(query);
        }

        // GET: Products/ListTable
        public async Task<IActionResult> ProductListTable(string orderby, string orderbydesc, int page = 1)
        {
            ViewBag.orderby = orderby;
            ViewBag.orderbydesc = orderbydesc;
            List<Products> query = null;

            if (orderby != null)
            {
                switch (orderby.ToLower())
                {
                    case "serial":
                        query = await _context.Products.Select(p => new Products()
                        {
                            Serial = p.Serial,
                            ProductName = p.ProductName,
                            PurchasePrice = p.PurchasePrice,
                            SalePrice = p.SalePrice,
                            Quantity = p.Quantity,
                            Status = p.Status,
                            Deleted = p.Deleted
                        })
                        .Where(p => !(bool)p.Deleted)
                        .OrderBy(p => p.Serial)
                        .Skip((page - 1) * listLimits)
                        .Take(listLimits).ToListAsync();
                        break;
                    case "productname":
                        query = await _context.Products.Select(p => new Products()
                        {
                            Serial = p.Serial,
                            ProductName = p.ProductName,
                            PurchasePrice = p.PurchasePrice,
                            SalePrice = p.SalePrice,
                            Quantity = p.Quantity,
                            Status = p.Status,
                            Deleted = p.Deleted
                        })
                        .Where(p => !(bool)p.Deleted)
                        .OrderBy(p => p.ProductName)
                        .Skip((page - 1) * listLimits)
                        .Take(listLimits).ToListAsync();
                        break;
                    case "purchaseprice":
                        query = await _context.Products.Select(p => new Products()
                        {
                            Serial = p.Serial,
                            ProductName = p.ProductName,
                            PurchasePrice = p.PurchasePrice,
                            SalePrice = p.SalePrice,
                            Quantity = p.Quantity,
                            Status = p.Status,
                            Deleted = p.Deleted
                        })
                        .Where(p => !(bool)p.Deleted)
                        .OrderBy(p => p.PurchasePrice)
                        .Skip((page - 1) * listLimits)
                        .Take(listLimits).ToListAsync();
                        break;
                    case "saleprice":
                        query = await _context.Products.Select(p => new Products()
                        {
                            Serial = p.Serial,
                            ProductName = p.ProductName,
                            PurchasePrice = p.PurchasePrice,
                            SalePrice = p.SalePrice,
                            Quantity = p.Quantity,
                            Status = p.Status,
                            Deleted = p.Deleted
                        })
                        .Where(p => !(bool)p.Deleted)
                        .OrderBy(p => p.SalePrice)
                        .Skip((page - 1) * listLimits)
                        .Take(listLimits).ToListAsync();
                        break;
                    case "quantity":
                        query = await _context.Products.Select(p => new Products()
                        {
                            Serial = p.Serial,
                            ProductName = p.ProductName,
                            PurchasePrice = p.PurchasePrice,
                            SalePrice = p.SalePrice,
                            Quantity = p.Quantity,
                            Status = p.Status,
                            Deleted = p.Deleted
                        })
                        .Where(p => !(bool)p.Deleted)
                        .OrderBy(p => p.Quantity)
                        .Skip((page - 1) * listLimits)
                        .Take(listLimits).ToListAsync();
                        break;
                    case "Status":
                        query = await _context.Products.Select(p => new Products()
                        {
                            Serial = p.Serial,
                            ProductName = p.ProductName,
                            PurchasePrice = p.PurchasePrice,
                            SalePrice = p.SalePrice,
                            Quantity = p.Quantity,
                            Status = p.Status,
                            Deleted = p.Deleted
                        })
                        .Where(p => !(bool)p.Deleted)
                        .OrderBy(p => p.Status)
                        .Skip((page - 1) * listLimits)
                        .Take(listLimits).ToListAsync();
                        break;
                }
            }
            else if (orderbydesc != null)
            {
                switch (orderbydesc.ToLower())
                {
                    case "serial":
                        query = await _context.Products.Select(p => new Products()
                        {
                            Serial = p.Serial,
                            ProductName = p.ProductName,
                            PurchasePrice = p.PurchasePrice,
                            SalePrice = p.SalePrice,
                            Quantity = p.Quantity,
                            Status = p.Status,
                            Deleted = p.Deleted
                        })
                        .Where(p => !(bool)p.Deleted)
                        .OrderByDescending(p => p.Serial)
                        .Skip((page - 1) * listLimits)
                        .Take(listLimits).ToListAsync();
                        break;
                    case "productname":
                        query = await _context.Products.Select(p => new Products()
                        {
                            Serial = p.Serial,
                            ProductName = p.ProductName,
                            PurchasePrice = p.PurchasePrice,
                            SalePrice = p.SalePrice,
                            Quantity = p.Quantity,
                            Status = p.Status,
                            Deleted = p.Deleted
                        })
                        .Where(p => !(bool)p.Deleted)
                        .OrderByDescending(p => p.ProductName)
                        .Skip((page - 1) * listLimits)
                        .Take(listLimits).ToListAsync();
                        break;
                    case "purchaseprice":
                        query = await _context.Products.Select(p => new Products()
                        {
                            Serial = p.Serial,
                            ProductName = p.ProductName,
                            PurchasePrice = p.PurchasePrice,
                            SalePrice = p.SalePrice,
                            Quantity = p.Quantity,
                            Status = p.Status,
                            Deleted = p.Deleted
                        })
                        .Where(p => !(bool)p.Deleted)
                        .OrderByDescending(p => p.PurchasePrice)
                        .Skip((page - 1) * listLimits)
                        .Take(listLimits).ToListAsync();
                        break;
                    case "saleprice":
                        query = await _context.Products.Select(p => new Products()
                        {
                            Serial = p.Serial,
                            ProductName = p.ProductName,
                            PurchasePrice = p.PurchasePrice,
                            SalePrice = p.SalePrice,
                            Quantity = p.Quantity,
                            Status = p.Status,
                            Deleted = p.Deleted
                        })
                        .Where(p => !(bool)p.Deleted)
                        .OrderByDescending(p => p.SalePrice)
                        .Skip((page - 1) * listLimits)
                        .Take(listLimits).ToListAsync();
                        break;
                    case "quantity":
                        query = await _context.Products.Select(p => new Products()
                        {
                            Serial = p.Serial,
                            ProductName = p.ProductName,
                            PurchasePrice = p.PurchasePrice,
                            SalePrice = p.SalePrice,
                            Quantity = p.Quantity,
                            Status = p.Status,
                            Deleted = p.Deleted
                        })
                        .Where(p => !(bool)p.Deleted)
                        .OrderByDescending(p => p.Quantity)
                        .Skip((page - 1) * listLimits)
                        .Take(listLimits).ToListAsync();
                        break;
                    case "Status":
                        query = await _context.Products.Select(p => new Products()
                        {
                            Serial = p.Serial,
                            ProductName = p.ProductName,
                            PurchasePrice = p.PurchasePrice,
                            SalePrice = p.SalePrice,
                            Quantity = p.Quantity,
                            Status = p.Status,
                            Deleted = p.Deleted
                        })
                        .Where(p => !(bool)p.Deleted)
                        .OrderByDescending(p => p.Status)
                        .Skip((page - 1) * listLimits)
                        .Take(listLimits).ToListAsync();
                        break;
                }
            }

            if (query == null)
                query = await _context.Products
                .Where(p => !(bool)p.Deleted)
                .OrderBy(p => p.Serial)
                .Skip((page - 1) * listLimits)
                .Take(listLimits).ToListAsync();

            var total = _context.Products.Where(p => !p.Deleted.Value).Count();

            ViewBag.nextPage = true;
            ViewBag.totalRecord = total;
            ViewBag.totalPage = (int)Math.Ceiling(total * 1.0 / listLimits);
            ViewBag.currentPage = page;

            return PartialView(query);
        }

        // GET: Show
        public async Task<IActionResult> Show(int? page)
        {
            if (page == null) page = 1;

            List<Products> query = await _context.Products
                .Where(p => !(bool)p.Deleted)
                .OrderByDescending(p => p.Serial)
                .ToListAsync();

            for (int i = 0; i < query.Count(); i++)
            {
                var imageUrls = await _context.ProductImages
                    .Select(p => new ProductImages()
                    {
                        Id = p.Id,
                        Serial = p.Serial,
                        Url_image = p.Url_image
                    }).Where(p => p.Serial == query[i].Serial)
                .ToListAsync();
                query[i].ImageUrls = imageUrls;
            }

            ViewBag.trashNumber = _context.Products.Where(p => p.Deleted.Value).Count();
            return View(query);
        }

        // GET: Products
        public async Task<IActionResult> Search(string q, string productId)
        {
            if (_context.Products == null) return View(new List<Products>());

            var query = await _context.Products.Select(p => new Products()
            {
                Serial = p.Serial,
                ProductName = p.ProductName,
                PurchasePrice = p.PurchasePrice,
                SalePrice = p.SalePrice,
                Quantity = p.Quantity,
                Status = p.Status
            }).Where(p => p.ProductName.Contains(q.ToLower())).ToListAsync();

            for (int i = 0; i < query.Count(); i++)
            {
                var imageUrls = await _context.ProductImages.Select(p => new ProductImages()
                {
                    Id = p.Id,
                    Serial = p.Serial,
                    Url_image = p.Url_image
                }).Where(p => p.Serial == query[i].Serial)
                .ToListAsync();
                query[i].ImageUrls = imageUrls;
            }

            ViewBag.q = q;
            return View(query);
        }

        public async Task<List<Products>> SearchJson(string q, string productId)
        {
            if (_context.Products == null) return new List<Products>();
            if (productId != null)
            {
                var response = await _context.Products
                    .Where(p => p.Serial.ToLower().StartsWith(productId.ToLower())
                    || p.ProductName.ToLower().Contains(productId.ToLower())).ToListAsync();

                return response;
            }

            var query = await _context.Products.Select(p => new Products()
            {
                Serial = p.Serial,
                ProductName = p.ProductName,
                PurchasePrice = p.PurchasePrice,
                SalePrice = p.SalePrice,
                Quantity = p.Quantity,
                Status = p.Status
            }).Where(p => p.ProductName.Contains(q.ToLower())).ToListAsync();

            for (int i = 0; i < query.Count(); i++)
            {
                var imageUrls = await _context.ProductImages.Select(p => new ProductImages()
                {
                    Id = p.Id,
                    Serial = p.Serial,
                    Url_image = p.Url_image
                }).Where(p => p.Serial == query[i].Serial)
                .ToListAsync();
                query[i].ImageUrls = imageUrls;
            }
            return query;
        }
        // GET: Products/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .FirstOrDefaultAsync(m => m.Serial == id);

            if (products == null)
            {
                return NotFound();
            }

            var imageUrls = await _context.ProductImages.Select(p => new ProductImages()
            {
                Id = p.Id,
                Serial = p.Serial,
                Url_image = p.Url_image
            }).Where(p => p.Serial == products.Serial)
               .ToListAsync();

            products.ImageUrls = imageUrls;

            return View(products);
        }

        // GET: Products/Create
        [Authorize(Roles = "1, 2")]
        public IActionResult Create()
        {
            var query = _context.Products.Take(1).OrderByDescending(p => p.Serial).FirstOrDefault();
            var seriral = Convert.ToInt32(query?.Serial.Substring(1)) + 1;
            var seriralText = "";

            for (int i = 1; i <= 3 - seriral.ToString().Length; i++)
            {
                seriralText = "0" + seriral;
            }
            seriralText = "P" + seriralText;
            ViewBag.serial = seriralText;

            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [Authorize(Roles = "1, 2")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Serial,ProductName,PurchasePrice,SalePrice,Quantity,SourceId,Status")] Products products)
        {
            if (ModelState.IsValid)
            {
                products.Deleted = false;
                _context.Add(products);

                await _context.SaveChangesAsync();


                // Save files
                try
                {
                    var files = HttpContext.Request.Form.Files;

                    foreach (var image in files)
                    {
                        if (image != null && image.Length > 0)
                        {
                            var file = image;
                            var uploads = Path.Combine("wwwroot", "images", "uploaded");
                            if (file.Length > 0)
                            {
                                var fileName = file.FileName;
                                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);

                                    var newProductImage = new ProductImages()
                                    {
                                        Url_image = "/" + Path.Combine("images", "uploaded", fileName),
                                        Serial = products.Serial,
                                    };

                                    _context.Database.ExecuteSqlRaw(string.Format("INSERT Product_Images(Serial, Url_image) VALUES(N'{0}',N'{1}')",
                                        newProductImage.Serial,
                                        newProductImage.Url_image));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"\n\n[UPLOAD IMAGES ERROR]{ex.Message}\n\n");
                }

                // await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> Edit(string id,
            [Bind("Serial,ProductName,PurchasePrice,SalePrice,Quantity,SourceId,Status")] Products products)
        {
            if (id != products.Serial)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.Serial))
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
            return View(products);
        }

        private bool ProductsExists(string id)
        {
            return (_context.Products?.Any(e => e.Serial == id)).GetValueOrDefault();
        }


        #region Delete product
        [HttpPost]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> DeleteSoft(string id)
        {
            // TODO: Move the product which has
            // ProductId equals id to Bin trash (Set Deteted Prop to TRUE).

            if (_context.Products == null)
            {
                return Problem("Entity set 'ShowroomContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.Deleted = true;
                _context.Products.Update(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // GET: Products/Delete/5
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .FirstOrDefaultAsync(m => m.Serial == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        [HttpPost]
        [Authorize(Roles = "2")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ShowroomContext.Products'  is null.");
            }

            var products = await _context.Products.FindAsync(id);
            var productImages = await _context.ProductImages.Select(p => new ProductImages()
            {
                Id = p.Id,
                Serial = p.Serial,
                Url_image = p.Url_image
            }).Where(p => p.Serial == id).ToListAsync();
            var purchaseInvoices = await _context.PurchaseInvoices.Where(p => p.ProductId == id).ToListAsync();
            var salesInvoices = await _context.SalesInvoices.Where(p => p.ProductId == id).ToListAsync();

            if (productImages != null && productImages.Count > 0)
            {
                for (int i = 0; i < productImages.Count; i++)
                {
                    _context.ProductImages.Remove(productImages[i]);
                }
            }

            await _context.SaveChangesAsync();

            if (purchaseInvoices != null && purchaseInvoices.Count > 0)
            {
                for (int i = 0; i < purchaseInvoices.Count; i++)
                {
                    purchaseInvoices[i].ProductId = null;
                    _context.Update(purchaseInvoices[i]);
                }
            }

            await _context.SaveChangesAsync();

            if (salesInvoices != null && salesInvoices.Count > 0)
            {
                for (int i = 0; i < salesInvoices.Count; i++)
                {
                    salesInvoices[i].ProductId = null;
                    _context.Update(salesInvoices[i]);
                }
            }

            await _context.SaveChangesAsync();

            if (products != null)
            {
                _context.Products.Remove(products);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Trash(int? page, int? limits)
        {
            if (_context.Products == null) return BadRequest();
            if (page == null) page = 1;
            if (limits == null) limits = 10;

            var query = _context.Products.Select(p => new Products()
            {
                Serial = p.Serial,
                ProductName = p.ProductName,
                PurchasePrice = p.PurchasePrice,
                SalePrice = p.SalePrice,
                Quantity = p.Quantity,
                Status = p.Status,
                Deleted = p.Deleted,
            }).Where(p => (bool)p.Deleted)
            .Skip((page - 1).Value * limits.Value)
            .Take(limits.Value);

            ViewBag.trashNumber = _context.Products.Where(p => p.Deleted.Value).Count();

            return View(await query.Skip((page.Value - 1) * listLimits).Take(listLimits).ToListAsync());
        }


        [HttpPost]
        public async Task<IActionResult> UndoFromTrash(string id)
        {
            var products = _context
                .Products
                .Where(p => p.Serial == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                products.Deleted = false;

                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.Serial))
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
            return View(products);
        }
        #endregion
    }
}
