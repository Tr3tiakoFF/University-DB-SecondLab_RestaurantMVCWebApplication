using System.ComponentModel.DataAnnotations;

namespace RestaurantWebApplication
{
    public partial class Product
    {
        public Product()
        {
            DishesProducts = new HashSet<DishesProduct>();
        }

        public int ProductId { get; set; }
        [Display(Name = "NAME")]
        [Required(ErrorMessage = "This field should not be empty")]
        public string Name { get; set; } = null!;
        [Display(Name = "PRODUCT TYPE")]
        [Required(ErrorMessage = "This field should not be empty")]
        public int ProductTypeId { get; set; }

        [Display(Name = "PRODUCT TYPE")]
        public virtual ProductType ProductType { get; set; } = null!;
        public virtual ICollection<DishesProduct> DishesProducts { get; set; }
    }
}
