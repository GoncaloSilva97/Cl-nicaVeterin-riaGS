using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinicGS.Data.Entity;

namespace VeterinaryClinicGS.Data
{
    public interface IAnimalsRepository : IGenericRepository<Animals>
    {
        public IQueryable GetAllWithOwner();
    }
}
