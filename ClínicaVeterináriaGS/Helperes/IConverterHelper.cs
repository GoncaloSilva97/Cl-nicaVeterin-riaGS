using VeterinaryClinicGS.Data.Entity;
using VeterinaryClinicGS.Models;
using System;
using System.Threading.Tasks;

namespace VeterinaryClinicGS.Helperes
{
    public interface IConverterHelper
    {
        Task<Animals> ToAnimalAsync(AnimalViewModel model, Guid imageId, bool isNew);

        AnimalViewModel ToAnimalViewModel(Animals animal);

        

        //AnimalsResponse ToPetResponse(Animals animal);

        //OwnerResponse ToOwnerResposne(Owners owner);

    }
}
