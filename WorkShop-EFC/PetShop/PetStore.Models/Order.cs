using PetStore.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PetStore.Models
{
    public class Order
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [MinLength(GlobalConstants.OrderTownNameMinLenght)]
        public string Town { get; set; }

        public string Notes { get; set; }
        [Required]
        [MinLength(GlobalConstants.OrderAddressMinLenght)]
        public string Address { get; set; }

      
        public virtual ICollection<ClientProduct> ClientProducts { get; set; } = new HashSet<ClientProduct>();

        public decimal TotalPrice => this.ClientProducts.Sum(cp => cp.Product.Price * cp.Quantity);
    }
}
