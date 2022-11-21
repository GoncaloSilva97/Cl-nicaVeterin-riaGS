using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using VeterinaryClinicGS.Data;

namespace VeterinaryClinicGS.Helperes
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _dataContext;

        public CombosHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<SelectListItem> GetComboAnimalTypes()
        {
            var list = _dataContext.AnimalTypes.Select(pt => new SelectListItem
            {
                Text = pt.Name,
                Value = $"{pt.Id}"
            })
                .OrderBy(pt => pt.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a Animal type...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboServiceTypes()
        {
            var list = _dataContext.ServiceTypes.Select(pt => new SelectListItem
            {
                Text = pt.Name,
                Value = $"{pt.Id}"
            })
                .OrderBy(pt => pt.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a service type...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboOwners()
        {
            var list = _dataContext.Owners.Select(p => new SelectListItem
            {
                Text = p.User.FullName,
                Value = p.Id.ToString()
            }).OrderBy(p => p.Value).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select an owner...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboAnimals(int serviceTypeId)
        {
            var list = _dataContext.Animals.Where(p => p.Owner.Id == serviceTypeId).Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            }).OrderBy(p => p.Value).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select an animal...)",
                Value = "0"
            });

            return list;
        }




        public IEnumerable<SelectListItem> GetComboRooms(int serviceTypeId)
        {
            var list = _dataContext.Rooms.Where(r => r.ServiceType.Id == serviceTypeId).Select(r => new SelectListItem
            {
                Text = r.RoomNumber.ToString(),
                Value = r.Id.ToString()
            }).OrderBy(r => r.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a room...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboDoctor(int doctorId)
        {
            var list = _dataContext.Doctors.Where(d => d.ServiceType.Id == doctorId).Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id.ToString()
            }).OrderBy(d => d.Value).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a doctor...)",
                Value = "0"
            });

            return list;
        }

    }
}
