using Microsoft.EntityFrameworkCore;
using Shopping_API.Models;

namespace Shopping_API.Data
{

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.email, u.userName, u.phoneNumber })
                .IsUnique(true);
           
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique(true);
           
            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.ChildCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithMany(p => p.Categories)
                .UsingEntity(j => j.ToTable("CategoryProducts"));


            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name)
                .IsUnique(true);
        }
    }
}