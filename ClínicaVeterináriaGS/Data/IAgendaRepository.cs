using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinicGS.Data.Entity;

namespace VeterinaryClinicGS.Data
{
    public interface IAgendaRepository : IGenericRepository<Agenda>
    {
     


        //Task SearchDaysAsync(int days);
        Task AddDaysAsync(int days);


       
 









        //IEnumerable<SelectListItem> GetComboServiceTypes();
        //IEnumerable<SelectListItem> GetComboRooms();

        //IEnumerable<SelectListItem> GetComboDoctors();

        //IEnumerable<SelectListItem> GetComboAnimals();
        //IEnumerable<SelectListItem> GetComboOwners();
    }
}
