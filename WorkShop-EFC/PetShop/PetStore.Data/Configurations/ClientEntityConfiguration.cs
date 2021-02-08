using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Common;
using PetStore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetStore.Data.Configurations
{
    public class ClientEntityConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder
                   .Property(c => c.UserName)
                   .HasMaxLength(GlobalConstants.ClientUserNameMaxLenght)
                   .IsUnicode(false);

            builder
              .Property(b => b.Email)
              .HasMaxLength(GlobalConstants.ClientEmailMaxLenght)
              .IsUnicode(false);

            builder
                .Property(b => b.FirtsName)
                .HasMaxLength(GlobalConstants.ClientFirtsNameMaxLenght)
                .IsUnicode();

            builder
                .Property(b => b.LastName)
                .HasMaxLength(GlobalConstants.ClientLastNameMaxLenght)
                .IsUnicode();
        }
    }
}
