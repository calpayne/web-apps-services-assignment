using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    public class FoodMenuDTO
    {
        [Display(Name = "Id"), Required]
        public int Id { get; set; }

        [Display(Name = "Menu Name"), Required]
        public string MenuName { get; set; }

        [Display(Name = "Menu Description"), Required]
        public string MenuDescription { get; set; }

        [Display(Name = "People"), Required]
        public int People { get; set; }

        [Display(Name = "Menu Cost"), Required]
        public double MenuCost { get; set; }
    }
}
