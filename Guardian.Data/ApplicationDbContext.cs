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
        public DbSet<GuardianModel> Guardians { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuardianModel>().HasData(
                new GuardianModel()
                {
                    Id = 1,
                    Name = "Felippe Delesporte",
                    Age = 22,
                    Occupancy = "Software Developer",
                    Adress = "São Paulo",
                    CreatedDate = DateTime.Now,
                    Status = true,
                    UpdatedDate = null,
                    DeletedeDate = null                   
                });
        }
    }
}
