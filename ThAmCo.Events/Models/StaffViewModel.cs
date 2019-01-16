using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    public class StaffViewModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required, Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, Display(Name = "First Aider")]
        public bool FirstAider { get; set; }

        [Display(Name = "Events they're Staffing")]
        public List<StaffBookingViewModel> Staffings { get; set; }
    }
}
