using Cinema.Data.Models.Constant;
using Cinema.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Data.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.MaxLenghtTitle, MinimumLength = GlobalConstants.MinLenghtTitle)]
        public string Title { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        [Range(GlobalConstants.MinLRating,GlobalConstants.MaxLRating)]
        public double Rating { get; set; }

        [Required]
        [StringLength(GlobalConstants.MaxLenghtDirector, MinimumLength = GlobalConstants.MinLenghtDirector)]
        public string Director { get; set; }

        public ICollection<Projection> Projection { get; set; } = new HashSet<Projection>();
    }
}
