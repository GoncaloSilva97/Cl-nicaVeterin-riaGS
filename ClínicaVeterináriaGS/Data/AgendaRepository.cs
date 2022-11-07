

using Microsoft.EntityFrameworkCore;
using System.Linq;
using VeterinaryClinicGS.Data.Entity;

namespace VeterinaryClinicGS.Data
{
    public class AgendaRepository : GenericRepository<Agenda>, IAgendaRepository
    {

       

        public AgendaRepository(DataContext context) : base(context)
        {
          
        }






    }
}
