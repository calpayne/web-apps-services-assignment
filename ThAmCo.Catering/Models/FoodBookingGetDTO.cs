using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Catering.Models
{
    public class FoodBookingGetDTO
    {
        public string Id { get; set; }

        public int EventId { get; set; }

        public DateTime EventDate { get; set; }

        public DateTime WhenMade { get; set; }

        public int FoodMenuId { get; set; }

        public FoodMenuGetDTO FoodMenu { get; set; }
    }
}
