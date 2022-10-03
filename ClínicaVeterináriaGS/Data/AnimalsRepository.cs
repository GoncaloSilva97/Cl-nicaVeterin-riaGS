

using Microsoft.EntityFrameworkCore;
using System.Linq;
using VeterinaryClinicGS.Data.Entity;

namespace VeterinaryClinicGS.Data
{
    public class AnimalsRepository : GenericRepository<Animals>, IAnimalsRepository
    {

        public readonly DataContext _context;

        public AnimalsRepository(DataContext context) : base(context)
        {
            _context = context;
        }



        public IQueryable GetAllWithOwner()
        {
            return _context.Animals.Include(p => p.Owner);
        }



    }
}
