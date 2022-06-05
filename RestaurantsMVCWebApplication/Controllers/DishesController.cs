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
    public class DishesController : Controller
    {
        private readonly DBRestaurantsLiteContext _context;

        public DishesController(DBRestaurantsLiteContext context)
        {
            _context = context;
        }

        // GET: Dishes
        public async Task<IActionResult> Index()
        {

            List<Tuple<Dish, List<Restaurant>>> dishesWithRestaurants = new List<Tuple<Dish, List<Restaurant>>>();
            foreach (Dish d in _context.Dishes.ToList())
            {
                dishesWithRestaurants.Add(new Tuple<Dish, List<Restaurant>>(d, FindRestaurants(d.DishId)));
            }

            ViewBag.DishesWithRestaurants = dishesWithRestaurants;

              return _context.Dishes != null ? 
                          View(await _context.Dishes.ToListAsync()) :
                          Problem("Entity set 'DBRestaurantsLiteContext.Dishes'  is null.");
        }

        public List<Restaurant> FindRestaurants(int? dishId)
        {
            if (dishId == null)
                return null;

            List<Restaurant> restaurants =
                (from r in _context.Restaurants
                 where r.IconicDishId == dishId
                 select r).ToList();
            return restaurants;
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Dishes == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .FirstOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }

            ViewBag.Products = FindProducts(dish.DishId);

            return View(dish);
        }
        public async Task<IActionResult> Products(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .FirstOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }

            ViewBag.Products = _context.Products.ToList();
            ViewBag.DishProducts = FindProducts(dish.DishId);
            ViewBag.DishProductsWithTypes = FindProductsWithTypes(dish.DishId);


            return View(dish);
        }

        public List<Tuple<Product, ProductType>> FindProductsWithTypes(int? dishID)
        {
            if (dishID == null)
                return null;

            List<Product> products = _context.Products.ToList();

            List<Tuple<Product, ProductType>> productsWithTypes = new List<Tuple<Product, ProductType>>();

            foreach (Product prod in products)
            {
                int productTypeId = (from pt in _context.ProductTypes
                                     where pt.ProductTypeId == prod.ProductTypeId
                                     select pt.ProductTypeId).FirstOrDefault();
                ProductType productType = _context.ProductTypes.FindAsync(productTypeId).Result;
                productsWithTypes.Add(new Tuple<Product, ProductType>(prod, productType));
            }

            return productsWithTypes;
        }

        private List<Product> FindProducts(int? dishId)
        {
            if (dishId == null)
                return null;

            List<Product> products =
                (from prod in _context.Products
                 where prod.DishesProducts.Any(d => d.DishId == dishId)
                 select prod).ToList();
            return products;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct([Bind("PairId,DishId,ProductId")] DishesProduct dishesProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dishesProduct);
                await _context.SaveChangesAsync();
            }

            if (dishesProduct == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.FindAsync(dishesProduct.DishId);
            if (dish == null)
            {
                return NotFound();
            }

            return RedirectToAction("Products", "Dishes", new { id = dishesProduct.DishId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProduct([Bind("PairId,DishId,ProductId")] DishesProduct tempDishesProduct)
        {
            int id = (from dp in _context.DishesProducts
                      where dp.DishId == tempDishesProduct.DishId && dp.ProductId == tempDishesProduct.ProductId
                      select dp.PairId).FirstOrDefault();
            var dishesProduct = await _context.DishesProducts.FindAsync(id);
            
            if (ModelState.IsValid)
            {
                _context.DishesProducts.Remove(dishesProduct);
                await _context.SaveChangesAsync();
            }

            if (tempDishesProduct == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.FindAsync(tempDishesProduct.DishId);
            if (dish == null)
            {
                return NotFound();
            }

            return RedirectToAction("Products", "Dishes", new { id = dishesProduct.DishId });
        }

        // GET: Dishes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DishId,Name,Description")] Dish dish)
        {
            List<Dish> td = (from d in _context.Dishes
                             where d.Name == dish.Name
                             select d).ToList();
            if (td.Count != 0)
            {
                ModelState.AddModelError("Name", "THIS DISH ALREADY EXIST");
            }

            if (ModelState.IsValid)
            {
                _context.Add(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dish);
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Dishes == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DishId,Name,Description")] Dish dish)
        {
            if (id != dish.DishId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.DishId))
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
            return View(dish);
        }

        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Dishes == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .FirstOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                var dish = await _context.Dishes.FindAsync(id);
                foreach (Product p in FindProducts(id))
                {
                    int _id = (from _dp in _context.DishesProducts
                               where _dp.ProductId == p.ProductId && _dp.DishId == dish.DishId
                               select _dp.PairId).FirstOrDefault();
                    var dishesProducts = await _context.DishesProducts.FindAsync(_id);
                    _context.DishesProducts.Remove(dishesProducts);
                    await _context.SaveChangesAsync();
                }
                _context.Dishes.Remove(dish);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
          return (_context.Dishes?.Any(e => e.DishId == id)).GetValueOrDefault();
        }
    }
}
