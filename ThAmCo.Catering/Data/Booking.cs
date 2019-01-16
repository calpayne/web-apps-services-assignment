using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Catering.Controllers
{
    public class Booking
    {
        [Key]
        public string Id { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        public DateTime WhenMade { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public int FoodMenuId { get; set; }

        [ForeignKey(nameof(FoodMenuId))]
        public FoodMenu FoodMenu { get; set; }
    }
}
