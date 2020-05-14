using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SLParkApi.Models
{
    public class NationalPark
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string State { get; set; }

        public DateTime Careated { get; set; }

        public DateTime Established { get; set; }
    }
}
