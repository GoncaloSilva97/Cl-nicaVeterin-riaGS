using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using VeterinaryClinicGS.Data;
using VeterinaryClinicGS.Helperes;

using VeterinaryClinicGS.Models;

namespace VeterinaryClinicGS.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AgendaController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        
        private readonly IOwnersRepository _ownersRepository;
        private readonly IAnimalsRepository _animalsRepository;
        private readonly IAgendaHelper _agendaHelper;
        private readonly IAgendaRepository _agendaRepository;

      
        public AgendaController(
             DataContext context,
            IUserHelper userHelper,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
           
            IOwnersRepository ownersRepository,
            IAgendaHelper agendaHelper,
            IAgendaRepository agendaRepository,
            IAnimalsRepository animalsRepository)
        {
            _dataContext = context;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
           
            _ownersRepository = ownersRepository;
            _animalsRepository = animalsRepository;
            _agendaHelper = agendaHelper;
            _agendaRepository = agendaRepository;
        }
        

        public IActionResult Index()
        {
            return View(_dataContext.Agendas
                .Include(a => a.Doctor)
                .ThenInclude(s => s.ServiceType)
                .Include(s => s.Room)
                .Include(a => a.Owner)
                .ThenInclude(o => o.User)
                .Include(a => a.Animal)
                .Where(a => a.Date >= DateTime.Today));


        }

        public async Task<IActionResult> AddDays()
        {
            await _agendaHelper.AddDaysAsync(7);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Assing(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agenda = await _dataContext.Agendas
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (agenda == null)
            {
                return NotFound();
            }

            var model = new AgendaViewModel
            {
                Id = agenda.Id,
                Owners = _combosHelper.GetComboOwners(),
                Animals = _combosHelper.GetComboAnimals(0),
                Rooms = _combosHelper.GetComboRooms(),
                Doctors = _combosHelper.GetComboDoctor(),
            };

            return View(model);
        }

        [HttpPost]
       
        public async Task<IActionResult> Assing(AgendaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var agenda = await _agendaRepository.GetByIdAsync(model.Id);
                if (agenda != null)
                {
                    agenda.IsAvailable = false;
                    agenda.Owner = await _ownersRepository.GetByIdAsync(model.OwnerId);
                    agenda.Animal = await _animalsRepository.GetByIdAsync(model.AnimalId);
                    //agenda.Room = await _roomsRepository.GetByIdAsync(model.RoomId);
                    //agenda.Doctor = await _doctorsRepository.GetByIdAsync(model.DoctorId);


                    agenda.Remarks = model.Remarks;
                   await _agendaRepository.UpdateAsync(agenda);
                    //await _dataContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            model.Owners = _combosHelper.GetComboOwners();
            model.Animals = _combosHelper.GetComboAnimals(model.OwnerId);
            model.Rooms = _combosHelper.GetComboRooms();
            model.Doctors = _combosHelper.GetComboDoctor();
            return View(model);
        }

        public async Task<JsonResult> GetAnimalsAsync(int ownerId)
        {
            var Animals = await _dataContext.Animals
                .Where(p => p.Owner.Id == ownerId)
                .OrderBy(p => p.Name)
                .ToListAsync();
            return Json(Animals);
        }

        public async Task<IActionResult> Unassign(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agenda = await _dataContext.Agendas
                .Include(a => a.Owner)
                .Include(a => a.Animal)
                .Include(a => a.Doctor)
                .Include(a => a.Room)
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (agenda == null)
            {
                return NotFound();
            }

            agenda.IsAvailable = true;
            agenda.Animal = null;
            agenda.Owner = null;
            agenda.Doctor = null;
            agenda.Room = null;
            agenda.Remarks = null;

            await _agendaRepository.UpdateAsync(agenda);
            //await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
