

using Microsoft.EntityFrameworkCore;
using System.Linq;
using VeterinaryClinicGS.Data.Entity;

namespace VeterinaryClinicGS.Data
{
    public class OwnersRepository : GenericRepository<Owners>, IOwnersRepository
    {

        public OwnersRepository(DataContext context) : base(context)
        {
            
        }



    }
}
