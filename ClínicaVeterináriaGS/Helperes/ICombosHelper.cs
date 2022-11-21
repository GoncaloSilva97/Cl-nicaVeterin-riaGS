using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VeterinaryClinicGS.Helperes
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboAnimalTypes();

        IEnumerable<SelectListItem> GetComboServiceTypes();

        IEnumerable<SelectListItem> GetComboOwners();

        IEnumerable<SelectListItem> GetComboAnimals(int ownerId);

        IEnumerable<SelectListItem> GetComboRooms(int roomId);

        IEnumerable<SelectListItem> GetComboDoctor(int doctorId);
    }
}