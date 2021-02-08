using Cinema.Data.Models;
using Cinema.Data.Models.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cinema.DataProcessor.ImportDto
{
    public class HallImportDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(GlobalConstants.MaxLenghtName, MinimumLength = GlobalConstants.MinLenghtName)]
        public string Name { get; set; }

        public bool Is4Dx { get; set; }

        public bool Is3D { get; set; }

        [Range(GlobalConstants.MinSeatsCount,Double.MaxValue)]
        public int Seats { get; set; } 
    }
}
