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
    public class ChefsController : Controller
    {
        private readonly DBRestaurantsLiteContext _context;

        public ChefsController(DBRestaurantsLiteContext context)
        {
            _context = context;
        }

        // GET: Chefs
        public async Task<IActionResult> Index()
        {
            var dBRestaurantsLiteContext = _context.Chefs.Include(c => c.BirthCity);

            List<Tuple<Chef, List<Restaurant>>> chefsWithInfo = new List<Tuple<Chef, List<Restaurant>>>();

            foreach (Chef c in _context.Chefs)
            {
                chefsWithInfo.Add(new Tuple<Chef, List<Restaurant>>(
                    c,
                    FindRestaurants(c.ChefId)));
            }

            ViewBag.ChefsWithInfo = chefsWithInfo;

            return View(await dBRestaurantsLiteContext.ToListAsync());
        }

        public List<Restaurant> FindRestaurants(int? chefID)
        {
            if (chefID == null)
                return null;

            return _context.Restaurants.Where(r => r.ChefId == chefID).ToList();
        }

        // GET: Chefs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Chefs == null)
            {
                return NotFound();
            }

            var chef = await _context.Chefs
                .Include(c => c.BirthCity)
                .FirstOrDefaultAsync(m => m.ChefId == id);
            if (chef == null)
            {
                return NotFound();
            }

            ViewBag.Restaurants = FindRestaurants(chef.ChefId);

            return View(chef);
        }

        // GET: Chefs/Create
        public IActionResult Create()
        {
            ViewData["BirthCityId"] = new SelectList(_context.Cities, "CityId", "Name");

            ViewData["BirthCityName"] = new SelectList(_context.Cities, "CityId", "Name");

            return View();
        }

        // POST: Chefs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChefId,FirstName,MiddleName,LastName,BirthCityId")] Chef chef)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chef);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BirthCityId"] = new SelectList(_context.Cities, "CityId", "Name", chef.BirthCityId);

            ViewData["BirthCityName"] = new SelectList(_context.Cities, "CityId", "Name", chef.BirthCityId);

            return View(chef);
        }

        // GET: Chefs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Chefs == null)
            {
                return NotFound();
            }

            var chef = await _context.Chefs.FindAsync(id);
            if (chef == null)
            {
                return NotFound();
            }
            ViewData["BirthCityId"] = new SelectList(_context.Cities, "CityId", "Name", chef.BirthCityId);

            ViewData["BirthCityName"] = new SelectList(_context.Cities, "CityId", "Name", chef.BirthCityId);
             
            return View(chef);
        }

        // POST: Chefs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChefId,FirstName,MiddleName,LastName,BirthCityId")] Chef chef)
        {
            if (id != chef.ChefId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chef);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChefExists(chef.ChefId))
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
            ViewData["BirthCityId"] = new SelectList(_context.Cities, "CityId", "Name", chef.BirthCityId);

            ViewData["BirthCityName"] = new SelectList(_context.Cities, "CityId", "Name", chef.BirthCityId);
             
            return View(chef);
        }

        // GET: Chefs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Chefs == null)
            {
                return NotFound();
            }

            var chef = await _context.Chefs
                .Include(c => c.BirthCity)
                .FirstOrDefaultAsync(m => m.ChefId == id);
            if (chef == null)
            {
                return NotFound();
            }

            return View(chef);
        }

        // POST: Chefs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Chefs == null)
            {
                return Problem("Entity set 'DBRestaurantsLiteContext.Chefs'  is null.");
            }
            var chef = await _context.Chefs.FindAsync(id);
            if (chef != null)
            {
                _context.Chefs.Remove(chef);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChefExists(int id)
        {
          return (_context.Chefs?.Any(e => e.ChefId == id)).GetValueOrDefault();
        }
    }
}
