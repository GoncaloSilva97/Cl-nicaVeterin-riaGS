using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VeterinaryClinicGS.Data.Entity;
using System.Linq;


namespace VeterinaryClinicGS.Data
{
    public class DataContext : IdentityDbContext<User>
    {
       
        public DbSet<Agenda> Agendas { get; set; }

  

        public DbSet<Owners> Owners { get; set; }

        public DbSet<Doctors> Doctors { get; set; }

        public DbSet<Rooms> Rooms { get; set; }

        public DbSet<Animals> Animals { get; set; }

        public DbSet<AnimalType> AnimalTypes { get; set; }

        public DbSet<ServiceTypes> ServiceTypes { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }


        
    }
}
