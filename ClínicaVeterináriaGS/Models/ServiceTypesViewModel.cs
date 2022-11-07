using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VeterinaryClinicGS.Data.Entity;

namespace VeterinaryClinicGS.Models
{
    public class ServiceTypesViewModel : ServiceTypes
    {
        

        public IEnumerable<SelectListItem> ServiceTypes { get; set; }
    }
}
