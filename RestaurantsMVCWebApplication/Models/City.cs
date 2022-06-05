using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantWebApplication
{
    public partial class City
    {
        public City()
        {
            Chefs = new HashSet<Chef>();
            Restaurants = new HashSet<Restaurant>();
        }

        public int CityId { get; set; }
        [Required(ErrorMessage = "This field should not be empty")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "NAME")]
        public string Name { get; set; } = null!;

        [Display(Name = "LOCATION")]
        public virtual Country? Country { get; set; } = null!;
        public virtual ICollection<Chef> Chefs { get; set; }
        public virtual ICollection<Restaurant> Restaurants { get; set; }
    }
}
