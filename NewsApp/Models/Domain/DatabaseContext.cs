using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NewsApp.Models.Domain
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {


        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<Category> Category { get; set; }
        public DbSet<NewsCategory> NewsCategory { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Comments> Comments { get; set; }


        // one to many relationship with comments and users.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.User);

            modelBuilder.Entity<Comments>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments);

            base.OnModelCreating(modelBuilder);
        }
    }
}


