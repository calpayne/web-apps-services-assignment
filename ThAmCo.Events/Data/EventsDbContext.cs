﻿using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Data
{
    public class EventsDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<GuestBooking> Guests { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<StaffBooking> StaffBooking { get; set; }

        private IHostingEnvironment HostEnv { get; }

        public EventsDbContext(DbContextOptions<EventsDbContext> options,
                               IHostingEnvironment env) : base(options)
        {
            HostEnv = env;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("thamco.events");

            builder.Entity<GuestBooking>()
                   .HasKey(b => new { b.CustomerId, b.EventId });

            builder.Entity<StaffBooking>()
                   .HasKey(sb => new { sb.StaffId, sb.EventId });

            builder.Entity<Customer>()
                   .HasMany(c => c.Bookings)
                   .WithOne(b => b.Customer)
                   .HasForeignKey(b => b.CustomerId);

            builder.Entity<Staff>()
                   .HasMany(s => s.Bookings)
                   .WithOne(sb => sb.Staff)
                   .HasForeignKey(sb => sb.StaffId);

            builder.Entity<Event>()
                   .HasMany(e => e.Bookings)
                   .WithOne(b => b.Event)
                   .HasForeignKey(b => b.EventId);

            builder.Entity<Event>()
                   .HasMany(e => e.StaffBookings)
                   .WithOne(b => b.Event)
                   .HasForeignKey(b => b.EventId);

            // seed data for debug / development testing
            if (HostEnv != null && HostEnv.IsDevelopment())
            {
                builder.Entity<Customer>().HasData(
                    new Customer { Id = 1, Surname = "Robertson", FirstName = "Robert", Email = "bob@example.com" },
                    new Customer { Id = 2, Surname = "Thornton", FirstName = "Betty", Email = "betty@example.com" },
                    new Customer { Id = 3, Surname = "Jellybeans", FirstName = "Jin", Email = "jin@example.com" }
                );

                builder.Entity<Staff>().HasData(
                    new Staff { Id = 1, Surname = "Payne", FirstName = "Callum", FirstAider = true },
                    new Staff { Id = 2, Surname = "Payne", FirstName = "Ethan", FirstAider = false }
                );

                builder.Entity<Event>().HasData(
                    new Event { Id = 1, Title = "Bob's Big 50", Date = new DateTime(2018, 10, 28), Duration = new TimeSpan(6, 0, 0), Type = "PTY", Disabled = null },
                    new Event { Id = 2, Title = "Best Wedding Yet", Date = new DateTime(2018, 11, 3), Duration = new TimeSpan(12, 0, 0), Type = "WED", Disabled = null }
                );

                builder.Entity<GuestBooking>().HasData(
                    new GuestBooking { CustomerId = 1, EventId = 1, Attended = true },
                    new GuestBooking { CustomerId = 2, EventId = 1, Attended = false },
                    new GuestBooking { CustomerId = 1, EventId = 2, Attended = false },
                    new GuestBooking { CustomerId = 3, EventId = 2, Attended = false }
                );

                builder.Entity<StaffBooking>().HasData(
                    new StaffBooking { StaffId = 1, EventId = 1 },
                    new StaffBooking { StaffId = 2, EventId = 1 },
                    new StaffBooking { StaffId = 1, EventId = 2 }
                );
            }
        }

    }
}
