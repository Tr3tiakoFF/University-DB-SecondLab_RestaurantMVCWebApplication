using System.ComponentModel.DataAnnotations;

namespace RestaurantWebApplication
{
    public partial class Country
    {
        public Country()
        {
            Cities = new HashSet<City>();
        }

        public int CountryId { get; set; }
        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "NAME")]
        public string Name { get; set; } = null!;

        public virtual ICollection<City> Cities { get; set; }
    }
}