

using Microsoft.EntityFrameworkCore;
using System.Linq;
using VeterinaryClinicGS.Data.Entity;

namespace VeterinaryClinicGS.Data
{
    public class OwnerRepository : GenericRepository<Owners>, IOwnerRepository
    {

        public readonly DataContext _context;

        public OwnerRepository(DataContext context) : base(context)
        {
            _context = context;
        }



        public IQueryable GetAllWithUsers()
        {
            return _context.Owners.Include(p => p.User);
        }



    }
}
