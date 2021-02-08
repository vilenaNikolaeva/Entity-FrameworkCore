using Cinema.Data.Models.Constant;
using Cinema.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Data.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.MaxLenghtFistName,MinimumLength =GlobalConstants.MinLenghtFirstName)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(GlobalConstants.MaxLenghtLastName, MinimumLength = GlobalConstants.MinLenghtLasttName)]
        public string LastName { get; set; }

        [Required]
        [Range(GlobalConstants.MinAge, GlobalConstants.MaxAge)]
        public int Age { get; set; }

        [Required]
        [Range(GlobalConstants.MinBalanceValue, Double.MaxValue)]
        public decimal Balance { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
