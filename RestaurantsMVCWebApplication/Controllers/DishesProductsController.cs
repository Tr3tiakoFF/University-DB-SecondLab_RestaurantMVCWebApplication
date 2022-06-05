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
    public class DishesProductsController : Controller
    {
        private readonly DBRestaurantsLiteContext _context;

        public DishesProductsController(DBRestaurantsLiteContext context)
        {
            _context = context;
        }

        // GET: DishesProducts
        public async Task<IActionResult> Index()
        {
            var dBRestaurantsLiteContext = _context.DishesProducts.Include(d => d.Dish).Include(d => d.Product);
            return View(await dBRestaurantsLiteContext.ToListAsync());
        }

        // GET: DishesProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DishesProducts == null)
            {
                return NotFound();
            }

            var dishesProduct = await _context.DishesProducts
                .Include(d => d.Dish)
                .Include(d => d.Product)
                .FirstOrDefaultAsync(m => m.PairId == id);
            if (dishesProduct == null)
            {
                return NotFound();
            }

            return View(dishesProduct);
        }

        // GET: DishesProducts/Create
        public IActionResult Create()
        {
            ViewData["DishId"] = new SelectList(_context.Dishes, "DishId", "Name");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name");
            return View();
        }

        // POST: DishesProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PairId,DishId,ProductId,Amount")] DishesProduct dishesProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dishesProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DishId"] = new SelectList(_context.Dishes, "DishId", "Name", dishesProduct.DishId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", dishesProduct.ProductId);
            return View(dishesProduct);
        }

        // GET: DishesProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DishesProducts == null)
            {
                return NotFound();
            }

            var dishesProduct = await _context.DishesProducts.FindAsync(id);
            if (dishesProduct == null)
            {
                return NotFound();
            }
            ViewData["DishId"] = new SelectList(_context.Dishes, "DishId", "Name", dishesProduct.DishId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", dishesProduct.ProductId);
            return View(dishesProduct);
        }

        // POST: DishesProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PairId,DishId,ProductId,Amount")] DishesProduct dishesProduct)
        {
            if (id != dishesProduct.PairId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dishesProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishesProductExists(dishesProduct.PairId))
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
            ViewData["DishId"] = new SelectList(_context.Dishes, "DishId", "Name", dishesProduct.DishId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", dishesProduct.ProductId);
            return View(dishesProduct);
        }

        // GET: DishesProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DishesProducts == null)
            {
                return NotFound();
            }

            var dishesProduct = await _context.DishesProducts
                .Include(d => d.Dish)
                .Include(d => d.Product)
                .FirstOrDefaultAsync(m => m.PairId == id);
            if (dishesProduct == null)
            {
                return NotFound();
            }

            return View(dishesProduct);
        }

        // POST: DishesProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DishesProducts == null)
            {
                return Problem("Entity set 'DBRestaurantsLiteContext.DishesProducts'  is null.");
            }
            var dishesProduct = await _context.DishesProducts.FindAsync(id);
            if (dishesProduct != null)
            {
                _context.DishesProducts.Remove(dishesProduct);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishesProductExists(int id)
        {
          return (_context.DishesProducts?.Any(e => e.PairId == id)).GetValueOrDefault();
        }
    }
}
