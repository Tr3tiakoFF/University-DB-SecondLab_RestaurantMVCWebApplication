using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantWebApplication
{
    public partial class Restaurant
    {
        public Restaurant()
        {
        }

        public int RestaurantId { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "RESTAURANT NAME")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "CITY")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "ADDRESS")]
        public string CorrectAdress { get; set; } = null!;

        [Display(Name = "RESTAURANT THEAM")]
        public string? MainThemeDefenition { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "ICONIC DISH")]
        public int? IconicDishId { get; set; }

        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "BRAND CHEF")]
        public int ChefId { get; set; }


        [NotMapped]
        public string? Address
        {
            get => City == null ? CorrectAdress : City.Name + ", " + CorrectAdress;
        }

        public virtual Chef Chef { get; set; } = null!;
        public virtual City City { get; set; } = null!;
        public virtual Dish? IconicDish { get; set; }
    }
}
