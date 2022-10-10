using VeterinaryClinicGS.Data.Entity;
using VeterinaryClinicGS.Models;
using System;
using ClínicaVeterináriaGS.Data.Entity;
using System.Threading.Tasks;

namespace VeterinaryClinicGS.Helperes
{
    public interface IConverterHelper
    {
        Task<Animals> ToAnimalAsync(AnimalViewModel model, Guid imageId, bool isNew);

        AnimalViewModel ToAnimalViewModel(Animals animal);

        Task<History> ToHistoryAsync(HistoryViewModel model, bool isNew);

        HistoryViewModel ToHistoryViewModel(History history);

        //AnimalsResponse ToPetResponse(Animals animal);

        //OwnerResponse ToOwnerResposne(Owners owner);

    }
}
