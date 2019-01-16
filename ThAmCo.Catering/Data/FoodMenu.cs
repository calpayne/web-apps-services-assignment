using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Catering.Controllers
{
    public class FoodMenu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string MenuName { get; set; }

        [Required]
        public string MenuDescription { get; set; }

        [Required]
        public int People { get; set; }

        [Required, Range(0.0, Double.MaxValue)]
        public double MenuCost { get; set; }
    }
}
