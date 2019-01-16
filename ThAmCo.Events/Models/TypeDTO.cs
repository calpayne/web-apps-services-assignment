using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    public class TypeDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
