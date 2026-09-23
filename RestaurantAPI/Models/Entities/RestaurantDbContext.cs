using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RestaurantAPI.Entities
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {

        }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.Dishes)
            .WithOne(d => d.Restaurant)
            .HasForeignKey(d => d.RestaurantId);

            modelBuilder.Entity<Address>()
            .HasOne(a => a.Restaurant)
            .WithOne(r => r.Address)
            .HasForeignKey<Restaurant>(r => r.AddressId);

            modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired();

            modelBuilder.Entity<Role>()
            .Property(r => r.Name)
            .IsRequired();
        }

    }
}