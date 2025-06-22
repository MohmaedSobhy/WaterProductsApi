using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WaterProducts.models;

namespace WaterProducts.data
{
    public class ApplicationData:IdentityDbContext<ApplicationUser>
    {
        public ApplicationData(DbContextOptions<ApplicationData> options)
         : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.UserName)
                .IsUnique(false);

            
            builder.Entity<ApplicationUser>().ToTable("Users");

            builder.Entity<FavouriteProducts>().HasKey(u=>new {u.productId,u.userId});

            builder.Entity<FavouriteProducts>()
            .HasOne(f => f.user)
            .WithMany(u => u.favourites);


            builder.Entity<Cart>()
                .HasMany(c => c.products)
                .WithOne();

            builder.Entity<ProductCart>()
            .HasOne(ci => ci.product)
            .WithMany();


            builder.Entity<Order>()
                .HasOne(o => o.user)
                .WithMany(u => u.orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
        );

            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        }

        public DbSet<Product> products { get; set; }
        public DbSet<FavouriteProducts> favourites { get; set; }
        
        public DbSet<ApplicationUser> users { get; set; }

        public DbSet<Cart> carts { get; set; }

        public DbSet<ProductCart> productsCart { get; set; }

        public DbSet<Order> orders { get; set; }
    }
}
