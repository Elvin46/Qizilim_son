using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Qizilim.az.Models.Entities;
using Qizilim.az.Models.Entities.Membreship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Qizilim.az.Models.Entities.Color;
using static Qizilim.az.Models.Entities.Images;
using static Qizilim.az.Models.Entities.Products;

namespace Qizilim.az.Models.DataContext
{
    public class QizilimDbContext : IdentityDbContext<QizilimUser, QizilimRole, int, QizilimUserClaim, QizilimUserRole, QizilimUserLogin, QizilimRoleClaim, QizilimUserToken>
    {
        public QizilimDbContext(DbContextOptions options)
               : base(options)
        {

        }

        public DbSet<Centers> Centers { get; set; }
        public DbSet<Shops> Shops { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<Eyar> Eyars { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Kateqoriya> Kateqoriya { get; set; }
        public DbSet<ProductEyar> ProductEyar { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<ProductColors> ProductColors { get; set; }
        public DbSet<ProductKateqoriya> ProductKateqoriya { get; set; }
        public DbSet<LikedProduct> LikedProducts { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<FollowUser> FollowUser { get; set; }
        public DbSet<Advertisement> Advertisement { get; set; }
        public DbSet<FollowersShops> FollowerShop { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //membership

            modelBuilder.Entity<ProductKateqoriya>(e =>
            {
                e.HasKey(k => new { k.KateqoriyaId, k.ProductId });
            });
            modelBuilder.Entity<ProductEyar>(e =>
            {
                e.HasKey(k => new { k.EyarId, k.ProductId });
            });

            modelBuilder.Entity<ProductImage>(e =>
            {
                e.HasKey(k => new { k.ImageId, k.ProductId });
            });

            modelBuilder.Entity<ProductColors>(e =>
            {
                e.HasKey(k => new { k.ColorId, k.ProductId });
            });

            modelBuilder.Entity<LikedProduct>(e =>
            {
                e.HasKey(k => new { k.ProductId, k.UserId });
            });
            modelBuilder.Entity<Follow>(e =>
            {
                e.HasKey(f => f.FollowerId);
            });
            modelBuilder.Entity<FollowUser>(e =>
            {
                e.HasKey(k => new { k.FollowerId, k.UserId });
            });
            //modelBuilder.Entity<FollowersShops>(e =>
            //{
            //    e.HasKey(f => f.FollowerId);
            //});



            modelBuilder.Entity<QizilimUser>(e =>
            {
                e.ToTable("Users", "Membership");
            });
            modelBuilder.Entity<QizilimRole>(e =>
            {
                e.ToTable("Roles", "Membership");
            });
            modelBuilder.Entity<QizilimUserClaim>(e =>
            {
                e.ToTable("UserClaims", "Membership");
            });
            modelBuilder.Entity<QizilimUserToken>(e =>
            {
                e.HasKey(k => new { k.UserId, k.LoginProvider, k.Name });
                e.ToTable("UserTokens", "Membership");
            });
            modelBuilder.Entity<QizilimUserLogin>(e =>
            {
                e.HasKey(k => new { k.UserId, k.LoginProvider, k.ProviderKey });
                e.ToTable("UserLogins", "Membership");
            });
            modelBuilder.Entity<QizilimRoleClaim>(e =>
            {
                e.ToTable("RoleClaims", "Membership");
            });
            modelBuilder.Entity<QizilimUserRole>(e =>
            {
                e.HasKey(k => new { k.UserId, k.RoleId });
                e.ToTable("UserRoles", "Membership");
            });
        }
    }
}
