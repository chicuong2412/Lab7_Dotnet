using Lab7_LeChiCuong_2131200001.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab7_LeChiCuong_2131200001.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Models.Book> Books { get; set; }
        public DbSet<Models.Author> Authors { get; set; }
        public DbSet<Models.Category> Categories { get; set; }
        public DbSet<Models.Loan> Loans { get; set; }
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Carousel> Carousels { get; set; }

        public DbSet<Role> Role { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships and other settings here if needed
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Authors)
                .WithMany(a => a.Books)
                .UsingEntity(j => j.ToTable("BookAuthors"));

            modelBuilder.Entity<Book>().
                HasMany(b => b.Categories)
                .WithMany(c => c.Books)
                .UsingEntity(j => j.ToTable("BookCategories"));

        }
    }
}
