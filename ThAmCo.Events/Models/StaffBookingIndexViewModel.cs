using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    public class StaffBookingIndexViewModel
    {
        public int EventId { get; set; }

        [Display(Name = "Total Staff:")]
        public int TotalStaff { get; set; }

        [Display(Name = "Has First Aider:")]
        public String HasFirstAider { get; set; }

        [Display(Name = "Has Enough Staff:")]
        public String HasEnoughStaff { get; set; }

        [Display(Name = "All Staff")]
        public List<StaffBookingViewModel> StaffMembers { get; set; }
    }
}
