using BookStore_Management_system.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore_Management_system.Data
{
    public class BookStoreDbContext : DbContext
    {
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options)
        {
           //options.UseSqlServer("Server=DESKTOP-BI2M0VE\\SQLEXPRESS01;Database=BookStoreDb;Trusted_Connection=True;");
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Category { get; set; }

    }
}
