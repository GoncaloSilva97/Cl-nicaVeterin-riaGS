

using Microsoft.EntityFrameworkCore;
using System.Linq;
using VeterinaryClinicGS.Data.Entity;

namespace VeterinaryClinicGS.Data
{
    public class AnimalTypesRepository : GenericRepository<AnimalType>, IAnimalTypesRepository
    {

       

        public AnimalTypesRepository(DataContext context) : base(context)
        {
          
        }






    }
}
