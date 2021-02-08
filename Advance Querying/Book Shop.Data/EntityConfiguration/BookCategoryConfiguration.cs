using Book_Shop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Book_Shop.Data.EntityConfiguration
{
    public class BookCategoryConfiguration : IEntityTypeConfiguration<BookCategory>
    {
        public void Configure(EntityTypeBuilder<BookCategory> builder)
        {
            builder.HasKey(b => new { b.BookId, b.CategoryId });

            builder.HasOne(b => b.Book)
                .WithMany(c => c.BookCategories)
                .HasForeignKey(b => b.BookId);

            builder.HasOne(c => c.Category)
                .WithMany(b => b.CategoryBooks)
                .HasForeignKey(c => c.CategoryId);
        }
    }
}
