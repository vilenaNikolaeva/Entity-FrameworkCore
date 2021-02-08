
using PetStore.Common;
using PetStore.Models.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Models
{
    public class Product
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
       
        [Required]
        [MinLength(GlobalConstants.ProductNameMinLenght)]
        public string Name { get; set; }
        public ProductType ProductType { get; set; }

        public decimal Price { get; set; }

    }
}
