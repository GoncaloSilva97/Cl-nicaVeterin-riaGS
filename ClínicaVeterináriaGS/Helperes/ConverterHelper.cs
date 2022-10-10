using VeterinaryClinicGS.Data.Entity;
using VeterinaryClinicGS.Models;
using System;
using VeterinaryClinicGS.Data;

using System.Threading.Tasks;
using ClínicaVeterináriaGS.Data.Entity;

namespace VeterinaryClinicGS.Helperes
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _dataContext;
        private readonly ICombosHelper _combosHelper;

        public ConverterHelper(
            DataContext dataContext,
            ICombosHelper combosHelper)
        {
            _dataContext = dataContext;
            _combosHelper = combosHelper;
        }

        public async Task<Animals> ToAnimalAsync(AnimalViewModel model, Guid imageId, bool isNew)
        {
            var animal = new Animals
            {
                Agendas = model.Agendas,
                Born = model.Born,
                Histories = model.Histories,
                Id = isNew ? 0 : model.Id,
                ImageId = imageId,
                Name = model.Name,
                Owner = await _dataContext.Owners.FindAsync(model.OwnerId),
                AnimalType = await _dataContext.AnimalTypes.FindAsync(model.AnimalTypeId),
                Race = model.Race,
                Remarks = model.Remarks
            };

            return animal;
        }

        

        public AnimalViewModel ToAnimalViewModel(Animals animal)
        {
            return new AnimalViewModel
            {
                Agendas = animal.Agendas,
                Born = animal.Born,
                Histories = animal.Histories,
                ImageId = animal.ImageId,
                Name = animal.Name,
                Owner = animal.Owner,
                AnimalType = animal.AnimalType,
                Race = animal.Race,
                Remarks = animal.Remarks,
                Id = animal.Id,
                OwnerId = animal.Owner.Id,
                AnimalTypeId = animal.AnimalType.Id,
                AnimalTypes = _combosHelper.GetComboPetTypes()
            };
        }

        public async Task<History> ToHistoryAsync(HistoryViewModel model, bool isNew)
        {
            return new History
            {
                Date = model.Date.ToUniversalTime(),
                Description = model.Description,
                Id = isNew ? 0 : model.Id,
                Animal = await _dataContext.Animals.FindAsync(model.AnimalId),
                Remarks = model.Remarks,
                ServiceType = await _dataContext.ServiceTypes.FindAsync(model.ServiceTypeId)
            };
        }

        public HistoryViewModel ToHistoryViewModel(History history)
        {
            return new HistoryViewModel
            {
                Date = history.Date,
                Description = history.Description,
                Id = history.Id,
                AnimalId = history.Animal.Id,
                Remarks = history.Remarks,
                ServiceTypeId = history.ServiceType.Id,
                ServiceTypes = _combosHelper.GetComboServiceTypes()
            };
        }

        //public PetResponse ToPetResponse(Pet pet)
        //{
        //    if (pet == null)
        //    {
        //        return null;
        //    }

        //    return new PetResponse
        //    {
        //        Born = pet.Born,
        //        Id = pet.Id,
        //        ImageUrl = pet.ImageFullPath,
        //        Name = pet.Name,
        //        PetType = pet.PetType.Name,
        //        Race = pet.Race,
        //        Remarks = pet.Remarks
        //    };
        //}

        //public OwnerResponse ToOwnerResposne(Owner owner)
        //{
        //    if (owner == null)
        //    {
        //        return null;
        //    }

        //    return new OwnerResponse
        //    {
        //        Address = owner.User.Address,
        //        Document = owner.User.Document,
        //        Email = owner.User.Email,
        //        FirstName = owner.User.FirstName,
        //        LastName = owner.User.LastName,
        //        PhoneNumber = owner.User.PhoneNumber
        //    };
        //}
    }
}
