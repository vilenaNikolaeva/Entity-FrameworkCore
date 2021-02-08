﻿using PetStore.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PetStore.Models
{
    public class ClientProduct
    {
        [Required]
        [ForeignKey(nameof(Client))]
        public string ClientId { get; set; }
        public virtual Client Client { get; set; }

        [Required]
        [ForeignKey(nameof(Product))]
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        [ForeignKey(nameof(Order))]
        public string OrderId { get; set; }
        public Order Order { get; set; }


        [Range(GlobalConstants.ClientProductMinQuantity,int.MaxValue)]
        public int Quantity { get; set; }

    }
}
