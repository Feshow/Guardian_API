using Guardian.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Guardian.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<LocalUser> LocalUser { get; set; }
        public DbSet<GuardianModel> Guardians { get; set; }
        public DbSet<GuardianTaskModel> GuardianTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);//That wiil create de key mapping that is needed for Application User (The class by itself does not has a key to the table)

            modelBuilder.Entity<GuardianTaskModel>().HasData(
                new GuardianTaskModel()
                {
                    Id = 1,
                    TaksName = "First Task",
                    Description = "Testing fist API by myself",
                    Category = 1,
                    Priority = 1,
                    Status = true,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = null
                }); ;
        }
    }
}
