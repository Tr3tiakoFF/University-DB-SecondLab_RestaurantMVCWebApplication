using Microsoft.EntityFrameworkCore;
using RestaurantsMVCWebApplication.Models;

namespace RestaurantWebApplication
{
    public partial class DBRestaurantsLiteContext : DbContext
    {
        public DBRestaurantsLiteContext()
        {
        }

        public DBRestaurantsLiteContext(DbContextOptions<DBRestaurantsLiteContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Chef> Chefs { get; set; } = null!;
        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Dish> Dishes { get; set; } = null!;
        public virtual DbSet<DishesProduct> DishesProducts { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductType> ProductTypes { get; set; } = null!;
        public virtual DbSet<Restaurant> Restaurants { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server = WINDOWS-A5JH6BA; Database = DBRestaurantsLite; Trusted_Connection = True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chef>(entity =>
            {
                entity.Property(e => e.ChefId).HasColumnName("ChefID");

                entity.Property(e => e.BirthCityId).HasColumnName("BirthCityID");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.HasOne(d => d.BirthCity)
                    .WithMany(p => p.Chefs)
                    .HasForeignKey(d => d.BirthCityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chefs_Cities");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cities_Countries");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.Property(e => e.DishId).HasColumnName("DishID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<DishesProduct>(entity =>
            {
                entity.HasKey(e => e.PairId);

                entity.ToTable("Dishes_Products");

                entity.Property(e => e.PairId).HasColumnName("PairID");

                entity.Property(e => e.Amount).HasMaxLength(50);

                entity.Property(e => e.DishId).HasColumnName("DishID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Dish)
                    .WithMany(p => p.DishesProducts)
                    .HasForeignKey(d => d.DishId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dishes_Products_Dishes");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.DishesProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dishes_Products_Products");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.ProductTypeId).HasColumnName("ProductTypeID");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products_ProductTypes");
            });

            modelBuilder.Entity<ProductType>(entity =>
            {
                entity.Property(e => e.ProductTypeId).HasColumnName("ProductTypeID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");

                entity.Property(e => e.ChefId).HasColumnName("ChefID");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.IconicDishId).HasColumnName("IconicDishID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Chef)
                    .WithMany(p => p.Restaurants)
                    .HasForeignKey(d => d.ChefId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Restaurants_Chefs");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Restaurants)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Restaurants_Cities");

                entity.HasOne(d => d.IconicDish)
                    .WithMany(p => p.Restaurants)
                    .HasForeignKey(d => d.IconicDishId)
                    .HasConstraintName("FK_Restaurants_Dishes");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public DbSet<RestaurantsMVCWebApplication.Models.Query>? Query { get; set; }
    }
}
