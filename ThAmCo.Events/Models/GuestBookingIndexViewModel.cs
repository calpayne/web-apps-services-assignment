using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    public class GuestBookingIndexViewModel
    {
        public int EventId { get; set; }

        [Display(Name = "Total Guests:")]
        public int TotalGuests { get; set; }

        [Display(Name = "All Guests")]
        public List<GuestBookingViewModel> AllGuests { get; set; }

    }
}
