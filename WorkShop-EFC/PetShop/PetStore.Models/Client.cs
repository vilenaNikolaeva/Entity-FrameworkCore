using PetStore.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetStore.Models
{
    public class Client
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MinLength(GlobalConstants.ClientUserNameMinLenght)]
        public string UserName { get; set; }
        [Required]
        public string  Password { get; set; }
        [Required]
        [MinLength(GlobalConstants.ClientEmailMinLenght)]
        public string  Email { get; set; }
        [Required]
        [MinLength(GlobalConstants.ClientFirtsNameMinLenght)]
        public string FirtsName { get; set; }
        [Required]
        [MinLength(GlobalConstants.ClientLastNameMinLenght)]
        public string LastName { get; set; }
        public virtual ICollection<Pet> PetsBuyed { get; set; } = new HashSet<Pet>();
    }
}
