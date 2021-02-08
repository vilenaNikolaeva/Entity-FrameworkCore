using PetStore.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetStore.Models
{
    public class Breed
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(GlobalConstants.BreedNameMinLenght)]
        public string Name { get; set; }
         
        public virtual ICollection<Pet> Pets { get; set; } = new HashSet<Pet>();
    }
}
