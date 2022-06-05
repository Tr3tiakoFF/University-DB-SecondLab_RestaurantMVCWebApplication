namespace RestaurantWebApplication
{
    public partial class DishesProduct
    {
        public int PairId { get; set; }
        public int DishId { get; set; }
        public int ProductId { get; set; }
        public string? Amount { get; set; }

        public virtual Dish? Dish { get; set; } = null!;
        public virtual Product? Product { get; set; } = null!;
    }
}
