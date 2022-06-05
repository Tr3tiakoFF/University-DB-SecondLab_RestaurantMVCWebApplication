using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantWebApplication;

namespace RestaurantsMVCWebApplication.Controllers
{
    public class ProductTypesController : Controller
    {
        private readonly DBRestaurantsLiteContext _context;

        public ProductTypesController(DBRestaurantsLiteContext context)
        {
            _context = context;
        }

        // GET: ProductTypes
        public async Task<IActionResult> Index()
        {
            List<Tuple<ProductType, List<Product>>> productTypesWithProducts = new List<Tuple<ProductType, List<Product>>>();

            foreach (ProductType pt in _context.ProductTypes)
            {
                productTypesWithProducts.Add(new Tuple<ProductType, List<Product>>(
                    pt,
                    FindProducts(pt.ProductTypeId)));
            }

            ViewBag.ProductTypesWithProducts = productTypesWithProducts;

            return _context.ProductTypes != null ? 
                          View(await _context.ProductTypes.ToListAsync()) :
                          Problem("Entity set 'DBRestaurantsLiteContext.ProductTypes'  is null.");
        }

        public List<Product> FindProducts(int? productTypeID)
        {
            List<Product> products = (from p in _context.Products
                                      where p.ProductTypeId == productTypeID
                                      select p).ToList();
            return products;
        }

        // GET: ProductTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductTypes == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductTypes
                .FirstOrDefaultAsync(m => m.ProductTypeId == id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        // GET: ProductTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductTypeId,Name")] ProductType productType)
        {
            List<ProductType> pt = (from t in _context.ProductTypes
                                    where t.Name == productType.Name
                                    select t).ToList();
            if (pt.Count != 0)
            {
                ModelState.AddModelError("Name", "THIS PRODUCT TYPE ALREADY EXIST");
            }
            if (ModelState.IsValid)
            {
                _context.Add(productType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productType);
        }

        // GET: ProductTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductTypes == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductTypes.FindAsync(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        // POST: ProductTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductTypeId,Name")] ProductType productType)
        {
            if (id != productType.ProductTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductTypeExists(productType.ProductTypeId))
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
            return View(productType);
        }

        // GET: ProductTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductTypes == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductTypes
                .FirstOrDefaultAsync(m => m.ProductTypeId == id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        // POST: ProductTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductTypes == null)
            {
                return Problem("Entity set 'DBRestaurantsLiteContext.ProductTypes'  is null.");
            }
            var productType = await _context.ProductTypes.FindAsync(id);
            if (productType != null)
            {
                _context.ProductTypes.Remove(productType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductTypeExists(int id)
        {
          return (_context.ProductTypes?.Any(e => e.ProductTypeId == id)).GetValueOrDefault();
        }
    }
}
