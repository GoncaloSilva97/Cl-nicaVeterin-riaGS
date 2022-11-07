using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

using VeterinaryClinicGS.Data.Entity;

namespace VeterinaryClinicGS.Models
{
    public class RoomsViewModel : Rooms
    {
        public IEnumerable<SelectListItem> ListServiceTypes { get; set; }
    }
}
