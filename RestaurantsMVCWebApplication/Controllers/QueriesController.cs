using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantWebApplication;
using RestaurantsMVCWebApplication.Models;

namespace RestaurantsMVCWebApplication.Controllers
{
    public class QueriesController : Controller
    {
        private readonly DBRestaurantsLiteContext _context;

        public QueriesController(DBRestaurantsLiteContext context)
        {
            _context = context;
        }

        // GET: Queries
        public async Task<IActionResult> Index()
        {
              return _context.Query != null ? 
                          View(await _context.Query.ToListAsync()) :
                          Problem("Entity set 'DBRestaurantsLiteContext.Query'  is null.");
        }

        // GET: Queries/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Query == null)
            {
                return NotFound();
            }

            var query = await _context.Query
                .FirstOrDefaultAsync(m => m.QueryId == id);
            if (query == null)
            {
                return NotFound();
            }

            return View(query);
        }

        private bool QueryExists(string id)
        {
          return (_context.Query?.Any(e => e.QueryId == id)).GetValueOrDefault();
        }
    }
}
