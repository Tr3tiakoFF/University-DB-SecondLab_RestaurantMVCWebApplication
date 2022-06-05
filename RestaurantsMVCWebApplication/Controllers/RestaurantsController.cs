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
    public class RestaurantsController : Controller
    {
        private readonly DBRestaurantsLiteContext _context;

        public RestaurantsController(DBRestaurantsLiteContext context)
        {
            _context = context;
        }

        // GET: Restaurants
        public async Task<IActionResult> Index()
        {
            var dBRestaurantsLiteContext = _context.Restaurants.Include(r => r.Chef).Include(r => r.City).Include(r => r.IconicDish);
            return View(await dBRestaurantsLiteContext.ToListAsync());
        }

        // GET: Restaurants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Restaurants == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants
                .Include(r => r.Chef)
                .Include(r => r.City)
                .Include(r => r.IconicDish)
                .FirstOrDefaultAsync(m => m.RestaurantId == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // GET: Restaurants/Create
        public IActionResult Create()
        {
            ViewData["ChefId"] = new SelectList(_context.Chefs, "ChefId", "FirstName");
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "Name");
            ViewData["IconicDishId"] = new SelectList(_context.Dishes, "DishId", "Name");

            ViewData["ChefName"] = new SelectList(_context.Chefs, "ChefId", "FullName");
            ViewData["CityName"] = new SelectList(_context.Cities, "CityId", "Name");
            ViewData["IconicDishName"] = new SelectList(_context.Dishes, "DishId", "Name");

            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RestaurantId,Name,CityId,CorrectAdress,MainThemeDefenition,IconicDishId,ChefId")] Restaurant restaurant)
        {
            List<Restaurant> tr = (from r in _context.Restaurants
                                   where r.Name == restaurant.Name &&
                                   r.CityId == restaurant.CityId &&
                                   r.CorrectAdress == restaurant.CorrectAdress
                                   select r).ToList();
            if (tr.Count != 0)
            {
                ModelState.AddModelError("Name", "THIS RESTAURANT ALREADY EXIST IN THIS CITY BY THIS ADDRESS");
                ModelState.AddModelError("CityId", "THIS RESTAURANT ALREADY EXIST IN THIS CITY BY THIS ADDRESS");
                ModelState.AddModelError("CorrectAdress", "THIS RESTAURANT ALREADY EXIST IN THIS CITY BY THIS ADDRESS");
            }

            if (ModelState.IsValid)
            {
                _context.Add(restaurant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChefId"] = new SelectList(_context.Chefs, "ChefId", "FirstName", restaurant.ChefId);
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "Name", restaurant.CityId);
            ViewData["IconicDishId"] = new SelectList(_context.Dishes, "DishId", "Name", restaurant.IconicDishId);

            ViewData["ChefName"] = new SelectList(_context.Chefs, "ChefId", "FullName", restaurant.ChefId);
            ViewData["CityName"] = new SelectList(_context.Cities, "CityId", "Name", restaurant.CityId);
            ViewData["IconicDishName"] = new SelectList(_context.Dishes, "DishId", "Name", restaurant.IconicDishId);

            return View(restaurant);
        }

        // GET: Restaurants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Restaurants == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            ViewData["ChefId"] = new SelectList(_context.Chefs, "ChefId", "FirstName", restaurant.ChefId);
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "Name", restaurant.CityId);
            ViewData["IconicDishId"] = new SelectList(_context.Dishes, "DishId", "Name", restaurant.IconicDishId);


            ViewData["ChefName"] = new SelectList(_context.Chefs, "ChefId", "FullName", restaurant.ChefId);
            ViewData["CityName"] = new SelectList(_context.Cities, "CityId", "Name", restaurant.CityId);
            ViewData["IconicDishName"] = new SelectList(_context.Dishes, "DishId", "Name", restaurant.IconicDishId);

            return View(restaurant);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RestaurantId,Name,CityId,CorrectAdress,MainThemeDefenition,IconicDishId,ChefId")] Restaurant restaurant)
        {
            if (id != restaurant.RestaurantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.RestaurantId))
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
            ViewData["ChefId"] = new SelectList(_context.Chefs, "ChefId", "FirstName", restaurant.ChefId);
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "Name", restaurant.CityId);
            ViewData["IconicDishId"] = new SelectList(_context.Dishes, "DishId", "Name", restaurant.IconicDishId);

            ViewData["ChefName"] = new SelectList(_context.Chefs, "ChefId", "FullName", restaurant.ChefId);
            ViewData["CityName"] = new SelectList(_context.Cities, "CityId", "Name", restaurant.CityId);
            ViewData["IconicDishName"] = new SelectList(_context.Dishes, "DishId", "Name", restaurant.IconicDishId);

            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Restaurants == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants
                .Include(r => r.Chef)
                .Include(r => r.City)
                .Include(r => r.IconicDish)
                .FirstOrDefaultAsync(m => m.RestaurantId == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Restaurants == null)
            {
                return Problem("Entity set 'DBRestaurantsLiteContext.Restaurants'  is null.");
            }
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant != null)
            {
                _context.Restaurants.Remove(restaurant);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantExists(int id)
        {
          return (_context.Restaurants?.Any(e => e.RestaurantId == id)).GetValueOrDefault();
        }
    }
}
