using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantWebApplication
{
    public partial class Dish
    {
        public Dish()
        {
            DishesProducts = new HashSet<DishesProduct>();
            Restaurants = new HashSet<Restaurant>();
        }

        public int DishId { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "DISH NAME")]
        public string Name { get; set; } = null!;
        [Display(Name = "DESCRIPTION")]
        public string? Description { get; set; }

        public virtual ICollection<DishesProduct> DishesProducts { get; set; }
        public virtual ICollection<Restaurant> Restaurants { get; set; }

    }
}
