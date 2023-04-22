using Guardian.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guardian.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<LocalUser> LocalUser { get; set; }
        public DbSet<GuardianModel> Guardians { get; set; }
        public DbSet<GuardianTaskModel> GuardianTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
