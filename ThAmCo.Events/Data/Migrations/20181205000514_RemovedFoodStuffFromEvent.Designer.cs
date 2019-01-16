﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Data.Migrations
{
    [DbContext(typeof(EventsDbContext))]
    [Migration("20181205000514_RemovedFoodStuffFromEvent")]
    partial class RemovedFoodStuffFromEvent
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("thamco.events")
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ThAmCo.Events.Data.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("Surname")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Customers");

                    b.HasData(
                        new { Id = 1, Email = "bob@example.com", FirstName = "Robert", Surname = "Robertson" },
                        new { Id = 2, Email = "betty@example.com", FirstName = "Betty", Surname = "Thornton" },
                        new { Id = 3, Email = "jin@example.com", FirstName = "Jin", Surname = "Jellybeans" }
                    );
                });

            modelBuilder.Entity("ThAmCo.Events.Data.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<DateTime?>("Disabled");

                    b.Property<TimeSpan?>("Duration");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("Type")
                        .IsRequired();

                    b.Property<string>("Venue");

                    b.Property<string>("VenueReference");

                    b.HasKey("Id");

                    b.ToTable("Events");

                    b.HasData(
                        new { Id = 1, Date = new DateTime(2018, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), Duration = new TimeSpan(0, 6, 0, 0, 0), Title = "Bob's Big 50", Type = "PTY" },
                        new { Id = 2, Date = new DateTime(2018, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), Duration = new TimeSpan(0, 12, 0, 0, 0), Title = "Best Wedding Yet", Type = "WED" }
                    );
                });

            modelBuilder.Entity("ThAmCo.Events.Data.GuestBooking", b =>
                {
                    b.Property<int>("CustomerId");

                    b.Property<int>("EventId");

                    b.Property<bool>("Attended");

                    b.HasKey("CustomerId", "EventId");

                    b.HasIndex("EventId");

                    b.ToTable("Guests");

                    b.HasData(
                        new { CustomerId = 1, EventId = 1, Attended = true },
                        new { CustomerId = 2, EventId = 1, Attended = false },
                        new { CustomerId = 1, EventId = 2, Attended = false },
                        new { CustomerId = 3, EventId = 2, Attended = false }
                    );
                });

            modelBuilder.Entity("ThAmCo.Events.Data.Staff", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("FirstAider");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("Surname")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Staff");

                    b.HasData(
                        new { Id = 1, FirstAider = true, FirstName = "Callum", Surname = "Payne" },
                        new { Id = 2, FirstAider = false, FirstName = "Ethan", Surname = "Payne" }
                    );
                });

            modelBuilder.Entity("ThAmCo.Events.Data.StaffBooking", b =>
                {
                    b.Property<int>("StaffId");

                    b.Property<int>("EventId");

                    b.HasKey("StaffId", "EventId");

                    b.HasAlternateKey("EventId", "StaffId");

                    b.ToTable("StaffBooking");

                    b.HasData(
                        new { StaffId = 1, EventId = 1 },
                        new { StaffId = 2, EventId = 1 },
                        new { StaffId = 1, EventId = 2 }
                    );
                });

            modelBuilder.Entity("ThAmCo.Events.Data.GuestBooking", b =>
                {
                    b.HasOne("ThAmCo.Events.Data.Customer", "Customer")
                        .WithMany("Bookings")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ThAmCo.Events.Data.Event", "Event")
                        .WithMany("Bookings")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ThAmCo.Events.Data.StaffBooking", b =>
                {
                    b.HasOne("ThAmCo.Events.Data.Event", "Event")
                        .WithMany("StaffBookings")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ThAmCo.Events.Data.Staff", "Staff")
                        .WithMany("Bookings")
                        .HasForeignKey("StaffId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
