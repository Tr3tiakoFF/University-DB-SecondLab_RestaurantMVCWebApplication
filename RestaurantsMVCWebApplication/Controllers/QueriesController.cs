using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantWebApplication;
using RestaurantsMVCWebApplication.Models;
using Microsoft.Data.SqlClient;

namespace RestaurantsMVCWebApplication.Controllers
{
    public class QueriesController : Controller
    {
        private const string CONN_STR = "Server= WINDOWS-A5JH6BA; Database=DBRestaurantsLite; Trusted_Connection=True; MultipleActiveResultSets=true; Encrypt=False";

        private const string S1_PATH = @"E:\WebApps\University\DB\Samples\Restaurants\RestaurantsMVCWebApplication\RestaurantsMVCWebApplication\Queries\S1.sql";
        private const string S2_PATH = @"E:\WebApps\University\DB\Samples\Restaurants\RestaurantsMVCWebApplication\RestaurantsMVCWebApplication\Queries\S2.sql";
        private const string S3_PATH = @"E:\WebApps\University\DB\Samples\Restaurants\RestaurantsMVCWebApplication\RestaurantsMVCWebApplication\Queries\S3.sql";
        private const string S4_PATH = @"E:\WebApps\University\DB\Samples\Restaurants\RestaurantsMVCWebApplication\RestaurantsMVCWebApplication\Queries\S4.sql";
        private const string S5_PATH = @"E:\WebApps\University\DB\Samples\Restaurants\RestaurantsMVCWebApplication\RestaurantsMVCWebApplication\Queries\S5.sql";

        private const string A1_PATH = @"E:\WebApps\University\DB\Samples\Restaurants\RestaurantsMVCWebApplication\RestaurantsMVCWebApplication\Queries\A1.sql";
        private const string A2_PATH = @"E:\WebApps\University\DB\Samples\Restaurants\RestaurantsMVCWebApplication\RestaurantsMVCWebApplication\Queries\A2.sql";
        private const string A3_PATH = @"E:\WebApps\University\DB\Samples\Restaurants\RestaurantsMVCWebApplication\RestaurantsMVCWebApplication\Queries\A3.sql";

        private readonly DBRestaurantsLiteContext _context;

        public QueriesController(DBRestaurantsLiteContext context)
        {
            _context = context;
        }

        // GET: Queries
        public async Task<IActionResult> Index()
        {
            ViewData["CountryNames"] = new SelectList(_context.Countries, "Name", "Name");
            ViewData["CityNames"] = new SelectList(_context.Cities, "Name", "Name");
            ViewData["RestaurantNames"] = new SelectList(_context.Restaurants, "Name", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery1(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S1_PATH);
            query = query.Replace("@", "\'" + queryModel.CountryName + "\'");

            queryModel.QueryId = "S1";

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        queryModel.OUTAmountOfRestaurants = Convert.ToInt32(result);
                    }
                    else
                    {
                        queryModel.ErrorFlag = 1;
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Details", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery2(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S2_PATH);
            query = query.Replace("@", "\'" + queryModel.RestaurantName + "\'");

            queryModel.QueryId = "S2";

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.OUTCityNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Details", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery3(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S3_PATH);
            query = query.Replace("@", "\'" + queryModel.CityName + "\'");

            queryModel.QueryId = "S3";

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.OUTDishNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Details", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery4(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S4_PATH);
            query = query.Replace("@", queryModel.AmountOfProducts.ToString());

            queryModel.QueryId = "S4";

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.OUTChefNames.Add(
                                reader.GetString(0) + " " +
                                reader.GetString(1) + " " +
                                reader.GetString(2));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Details", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery5(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S5_PATH);
            query = query.Replace("@", "\'" + queryModel.CityName + "\'");

            queryModel.QueryId = "S5";

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.OUTRestaurantNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Details", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdvancedQuery1(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(A1_PATH);

            queryModel.QueryId = "A1";

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.OUTCityNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Details", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdvancedQuery2(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(A2_PATH);
            query = query.Replace("@", "\'" + queryModel.RestaurantName + "\'");

            queryModel.QueryId = "A2";

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.OUTRestaurantNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Details", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdvancedQuery3(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(A3_PATH);
            query = query.Replace("@", "\'" + queryModel.CountryName + "\'");

            queryModel.QueryId = "A3";

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.OUTChefNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Details", queryModel);
        }

        // GET: Queries/Details/5
        public async Task<IActionResult> Details(Query queryModel)
        {
            return View(queryModel);
        }
    }
}
