using Microsoft.EntityFrameworkCore;

namespace Lab4.Models
{
    public class BookManagementContext : DbContext
    {
        public BookManagementContext(DbContextOptions<BookManagementContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Loans> Loans { get; set; }
        public DbSet<Carousel> Carousels { get; set; }
    }
}