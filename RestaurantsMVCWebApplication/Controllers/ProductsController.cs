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
    public class ProductsController : Controller
    {
        private readonly DBRestaurantsLiteContext _context;

        public ProductsController(DBRestaurantsLiteContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            List<Tuple<Product, List<Dish>>> productWithDishes = new List<Tuple<Product, List<Dish>>>();

            foreach (Product p in _context.Products)
            {
                productWithDishes.Add(new Tuple<Product, List<Dish>>(
                    p,
                    FindDishes(p.ProductId)));
            }

            ViewBag.ProductWithDishes = productWithDishes;

            var dBRestaurantsLiteContext = _context.Products.Include(p => p.ProductType);
            return View(await dBRestaurantsLiteContext.ToListAsync());
        }

        public List<Dish> FindDishes(int? productID)
        {
            List<Dish> dishes = (from d in _context.Dishes
                                 where d.DishesProducts.Any(dp => dp.ProductId == productID)
                                 select d).ToList();
            return dishes;
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "ProductTypeId", "ProductTypeId");

            ViewData["ProductTypeName"] = new SelectList(_context.ProductTypes, "ProductTypeId", "Name");

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,ProductTypeId")] Product product)
        {
            List<Product> tp = (from p in _context.Products
                                where p.Name == product.Name
                                select p).ToList();
            if (tp.Count != 0)
            {
                ModelState.AddModelError("Name", "THIS PRODUCT ALREADY EXIST");
            }
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "ProductTypeId", "ProductTypeId", product.ProductTypeId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "ProductTypeId", "ProductTypeId", product.ProductTypeId);

            ViewData["ProductTypeName"] = new SelectList(_context.ProductTypes, "ProductTypeId", "Name", product.ProductTypeId);

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,ProductTypeId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "ProductTypeId", "ProductTypeId", product.ProductTypeId);

            ViewData["ProductTypeName"] = new SelectList(_context.ProductTypes, "ProductTypeId", "Name", product.ProductTypeId);

            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'DBRestaurantsLiteContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
