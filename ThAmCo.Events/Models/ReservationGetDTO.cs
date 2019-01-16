using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    public class ReservationGetDTO
    {
        public string Reference { get; set; }

        public DateTime EventDate { get; set; }

        public string VenueCode { get; set; }

        [Display(Name = "Name")]
        public string VenueName { get; set; }

        [Display(Name = "Capacity")]
        public int VenueCapacity { get; set; }

        [Display(Name = "Cost per Hour")]
        public double VenueCostPerHour { get; set; }

        [Display(Name = "Reservation Created")]
        public DateTime WhenMade { get; set; }

        public string StaffId { get; set; }
    }
}
