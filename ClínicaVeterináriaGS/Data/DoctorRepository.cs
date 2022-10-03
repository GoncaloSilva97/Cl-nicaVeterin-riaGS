

using Microsoft.EntityFrameworkCore;
using System.Linq;
using VeterinaryClinicGS.Data.Entity;

namespace VeterinaryClinicGS.Data
{
    public class DoctorRepository : GenericRepository<Doctors>, IDoctorRepository
    {

        public readonly DataContext _context;

        public DoctorRepository(DataContext context) : base(context)
        {
            _context = context;
        }



        public IQueryable GetAllWithUsers()
        {
            return _context.Doctors.Include(p => p.User);
        }



    }
}
