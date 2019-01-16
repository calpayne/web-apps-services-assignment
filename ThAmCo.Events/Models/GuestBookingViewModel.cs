using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    public class GuestBookingViewModel
    {
        [Display(Name = "Customer Id")]
        public int CustomerId { get; set; }

        [Display(Name = "Customer")]
        public Customer Customer { get; set; }

        [Display(Name = "Event Id")]
        public int EventId { get; set; }

        [Display(Name = "Event")]
        public Event Event { get; set; }

        [Display(Name = "Event Name")]
        public string EventName { get; set; }

        [Display(Name = "Attended")]
        public bool Attended { get; set; }
    }
}
