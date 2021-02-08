using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Common;
using PetStore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetStore.Data.Configurations
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                .Property(o => o.Town)
                .HasMaxLength(GlobalConstants.OrderTownNameMaxLenght)
                .IsUnicode();

            builder
                .Property(o => o.Address)
                .HasMaxLength(GlobalConstants.OrderAddressMaxLenght)
                .IsUnicode();

            builder
                .Ignore(o => o.TotalPrice);
        }
    }
}
