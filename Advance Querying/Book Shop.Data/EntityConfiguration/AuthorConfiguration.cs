using Book_Shop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Book_Shop.Data.EntityConfiguration
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(a => a.AuthorId);

            builder.Property(a => a.FirstName)
                .HasMaxLength(50)
                .IsRequired(false)
                .IsUnicode();

            builder.Property(a => a.LastName)
                .HasMaxLength(50)
                .IsUnicode();

        }
    }
}
