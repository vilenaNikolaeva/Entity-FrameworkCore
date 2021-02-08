using Microsoft.EntityFrameworkCore;
using PetStore.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetStore.Data
{
    public class PetStoreDbContext : DbContext
    {
        public PetStoreDbContext (DbContextOptions options)
            : base(options)
        {
        }

        protected PetStoreDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
               optionsBuilder.UseSqlServer(DbConfiguration.ConnectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.ApplyConfigurationsFromAssembly
              (typeof(PetStoreDbContext).Assembly);
        }
    }
}
