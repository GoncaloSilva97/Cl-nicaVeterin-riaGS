using VeterinaryClinicGS.Data.Entity;

namespace VeterinaryClinicGS.Data
{
    public class ServiceTypesRepository : GenericRepository<ServiceTypes>, IServiceTypesRepository
    {

        public readonly DataContext _context;

        public ServiceTypesRepository(DataContext context) : base(context)
        {
            _context = context;
        }





    }
}
