using Book_Shop.Models;
using Microsoft.EntityFrameworkCore;
using Book_Shop.Data.EntityConfiguration;


namespace Book_Shop.Data
{
    public class BookShopContext : DbContext
    {
        public BookShopContext(DbContextOptions options) 
            : base(options)
        {
        }

        public BookShopContext()
        {
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionDatabase.ConnectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AuthorConfiguration());
            builder.ApplyConfiguration(new BookConfiguration());
            builder.ApplyConfiguration(new BookCategoryConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
        }
    }
}
