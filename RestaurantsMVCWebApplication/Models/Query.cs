using RestaurantWebApplication;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantsMVCWebApplication.Models
{
    public class Query
    {
        public string QueryId { get; set; }
        public string Error { get; set; }
        public int ErrorFlag { get; set; }

        //IN
        [NotMapped]
        public string CountryName { get; set; } = "ERROR";
        [NotMapped]
        public string CityName { get; set; } = "ERROR";
        [NotMapped]
        public string RestaurantName { get; set; } = "ERROR";

        [NotMapped]
        public int AmountOfProducts { get; set; } = 0;


        //OUT
        [NotMapped]
        public int OUTAmountOfRestaurants { get; set; } = 0;

        [NotMapped]
        public List<string> OUTCityNames { get; set; } = new List<string>();
        [NotMapped]
        public List<string> OUTRestaurantNames { get; set; } = new List<string>();
        [NotMapped]
        public List<string> OUTChefNames { get; set; } = new List<string>();
        [NotMapped]
        public List<string> OUTDishNames { get; set; } = new List<string>();
    }
}
