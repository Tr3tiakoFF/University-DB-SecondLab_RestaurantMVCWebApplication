using RestaurantWebApplication;

namespace RestaurantsMVCWebApplication.Models
{
    public class Query
    {
        public string QueryId { get; set; }
        public string Error { get; set; }
        public int ErrorFlag { get; set; }

        //IN
        public string CountryName { get; set; } = "ERROR";
        public string CityName { get; set; } = "ERROR";
        public string RestaurantName { get; set; } = "ERROR";

        public int AmountOfProducts { get; set; } = 0;


        //OUT
        public int OUTAmountOfRestaurants { get; set; } = 0;

        public List<string> OUTCityNames { get; set; } = new List<string>();
        public List<string> OUTRestaurantNames { get; set; } = new List<string>();
        public List<string> OUTChefNames { get; set; } = new List<string>();
        public List<string> OUTDishNames { get; set; } = new List<string>();
    }
}
