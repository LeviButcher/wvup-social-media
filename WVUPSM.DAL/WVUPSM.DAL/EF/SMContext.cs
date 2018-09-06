using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WVUPSM.Models.Entities;

namespace WVUPSM.DAL.EF
{
    public class SMContext : IdentityDbContext <IdentityUser>
    {

        private string connection = @"Server=(localdb)\mssqllocaldb;Database=WVUPSM;Trusted_Connection=True;MultipleActiveResultSets=true;";

        public DbSet<User> UserAccounts { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Post> Posts { get; set; }


        public SMContext()
        {

        }

        public SMContext(DbContextOptions options) : base (options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connection, options => options.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Post>(entity =>
           {
               entity.Property(e => e.DateCreated)
               .HasDefaultValueSql("getdate()");
           });

            builder.Entity<Follow>().HasKey(key => new { key.UserId, key.FollowId });

            base.OnModelCreating(builder);
        }
    }
}
