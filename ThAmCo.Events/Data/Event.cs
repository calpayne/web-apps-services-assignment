using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Data
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan? Duration { get; set; }

        [Required]
        public string Type { get; set; }

        public string Venue { get; set; }

        public string VenueReference { get; set; }

        public DateTime? Disabled { get; set; }

        public List<GuestBooking> Bookings { get; set; }

        public List<StaffBooking> StaffBookings { get; set; }
    }
}
