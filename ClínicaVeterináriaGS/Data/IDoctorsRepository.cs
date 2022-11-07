
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using VeterinaryClinicGS.Data.Entity;
using VeterinaryClinicGS.Models;

namespace VeterinaryClinicGS.Data
{
    public interface IDoctorsRepository : IGenericRepository<Doctors>
    {
        IEnumerable<SelectListItem> GetComboServiceTypes();


        Task<DoctorViewModel> AddServiceTypeAsync(DoctorViewModel model);

        Task<EditDoctorViewModel> EditServiceTypeAsync(EditDoctorViewModel model);
    }
}
