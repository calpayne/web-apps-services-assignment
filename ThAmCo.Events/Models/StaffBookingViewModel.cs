using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    public class StaffBookingViewModel
    {
        [Display(Name = "Staff Id")]
        public int StaffId { get; set; }

        [Display(Name = "Staff")]
        public Staff Staff { get; set; }

        [Display(Name = "Staff")]
        public String StaffName { get; set; }

        [Display(Name = "First Aider")]
        public String FirstAider { get; set; }

        [Display(Name = "Event Id")]
        public int EventId { get; set; }

        [Display(Name = "Event")]
        public Event Event { get; set; }

        [Display(Name = "Event Title")]
        public String EventTitle { get; set; }
    }
}
