﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    public class ReservationPostDTO
    {
        public DateTime EventDate { get; set; }

        public string VenueCode { get; set; }

        public string StaffId { get; set; }
    }
}
