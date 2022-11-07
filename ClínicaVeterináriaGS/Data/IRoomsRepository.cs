
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using VeterinaryClinicGS.Data.Entity;
using VeterinaryClinicGS.Models;

namespace VeterinaryClinicGS.Data
{
    public interface IRoomsRepository : IGenericRepository<Rooms>
    {
        IEnumerable<SelectListItem> GetComboServiceTypes();


        Task<RoomsViewModel> AddServiceTypeAsync(RoomsViewModel model);

        Task<RoomsViewModel> EditServiceTypeAsync(RoomsViewModel model);
    }
}
