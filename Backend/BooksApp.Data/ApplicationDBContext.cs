using BooksApp.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BooksApp.Data
{
    public class ApplicationDBContext : IdentityDbContext<UserEntity, IdentityRole, string>
    {
        public ApplicationDBContext(DbContextOptions options) : base(options) { }

        public DbSet<BookEntity> Books { get; set; }

        public DbSet<CategoryEntity> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<BookEntity>()
               .HasOne(b => b.Category)
               .WithMany(c => c.Books)
               .HasForeignKey(b => b.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
