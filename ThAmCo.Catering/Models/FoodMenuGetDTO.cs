using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Catering.Models
{
    public class FoodMenuGetDTO
    {
        public int Id { get; set; }

        public string MenuName { get; set; }

        public string MenuDescription { get; set; }

        public int People { get; set; }

        public double MenuCost { get; set; }
    }
}
