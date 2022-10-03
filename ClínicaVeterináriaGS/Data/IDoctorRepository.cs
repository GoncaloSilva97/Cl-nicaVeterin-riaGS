using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinicGS.Data.Entity;

namespace VeterinaryClinicGS.Data
{
    public interface IDoctorRepository : IGenericRepository<Doctors>
    {
        public IQueryable GetAllWithUsers();
    }
}
