using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VeterinaryClinicGS.Data.Entities;
using System.Linq;
using VeterinaryClinicGS.Data.Entity;
using ClínicaVeterináriaGS.Data.Entities;

namespace VeterinaryClinicGS.Data
{
    public class DataContext : IdentityDbContext<User>
    {
       
        public DbSet<Agenda> Agendas { get; set; }

        public DbSet<History> Histories { get; set; }

        public DbSet<Owners> Owners { get; set; }

        public DbSet<Doctors> Doctors { get; set; }

       

        public DbSet<Animals> Animals { get; set; }

        public DbSet<AnimalType> AnimalTypes { get; set; }

        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }


        
    }
}
