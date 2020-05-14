using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SLParkApi.Models;
using static SLParkApi.Models.Trail;

namespace SLParkApi.Models.Dtos
{
    public class TrialDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public double Distance { get; set; }

        public DifficultyType Difficulty { get; set; }

        [Required]
        public int NationalParkId { get; set; }

        public NationalParkDto NationalPark { get; set; }
    }
}
