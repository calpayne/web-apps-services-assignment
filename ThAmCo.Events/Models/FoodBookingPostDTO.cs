using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    public class FoodBookingPostDTO
    {
        public string Id { get; set; }

        public int EventId { get; set; }

        public DateTime EventDate { get; set; }

        public DateTime WhenMade { get; set; }

        public int FoodMenuId { get; set; }
    }
}
