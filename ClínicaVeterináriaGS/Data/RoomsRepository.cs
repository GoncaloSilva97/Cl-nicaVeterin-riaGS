

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinicGS.Data.Entity;
using VeterinaryClinicGS.Models;

namespace VeterinaryClinicGS.Data
{
    public class RoomsRepository : GenericRepository<Rooms>, IRoomsRepository
    {
        private readonly DataContext _context;
       
        public RoomsRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboServiceTypes()
        {
            var list = _context.ServiceTypes.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString(),

            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a Service Type...)",
                Value = "0",
            });

            return list;
        }

        public async Task<RoomsViewModel> AddServiceTypeAsync(RoomsViewModel model)
        {
            model.ServiceType = await _context.ServiceTypes.FindAsync(model.ServiceTypeId);

            return model;
        }


        public async Task<RoomsViewModel> EditServiceTypeAsync(RoomsViewModel model)
        {
            model.ServiceType = await _context.ServiceTypes.FindAsync(model.ServiceTypeId);

            return model;
        }

    }
}
