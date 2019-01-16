using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Catering.Controllers;

namespace ThAmCo.Catering.Data
{
    public class CateringDbContext : DbContext
    {
        public DbSet<FoodMenu> FoodMenus { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        private readonly IHostingEnvironment _hostEnv;

        public CateringDbContext(DbContextOptions<CateringDbContext> options,
                               IHostingEnvironment env) : base(options)
        {
            _hostEnv = env;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("thamco.catering");

            builder.Entity<FoodMenu>()
                   .HasKey(e => e.Id);

            builder.Entity<Booking>()
                   .HasKey(e => e.Id);

            builder.Entity<Booking>()
                   .HasOne(s => s.FoodMenu);

            if (_hostEnv != null && _hostEnv.IsDevelopment())
            {
                builder.Entity<FoodMenu>()
                       .HasData(
                            new FoodMenu { Id = 1, MenuName = "Sandwiches Buffet", MenuDescription = "Honey-roast ham with wholegrain mustard, Chicken Caesar salad sandwiches", People = 10, MenuCost = 24.99 },
                            new FoodMenu { Id = 2, MenuName = "Finger Buffet", MenuDescription = "Roast tomato and herb quiche, Smoked haddock & spring onion fishcake, Chicken fillet skewers with sweet chilli dipping sauce", People = 10, MenuCost = 24.99 },
                            new FoodMenu { Id = 3, MenuName = "Mini Desserts", MenuDescription = "Mini strawberry and cream pavlova, Fresh fruit skewer with chocolate dipping sauce", People = 10, MenuCost = 24.99 },
                            new FoodMenu { Id = 4, MenuName = "Late-night Snacks", MenuDescription = "Slider burger platter with pickles and sauces, Fish finger sandwich with tartare sauce", People = 10, MenuCost = 24.99 }
                       );
            }
        }
    }
}
