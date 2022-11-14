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
        
        private readonly IAgendaRepository _agendaRepository;
        private readonly IDoctorsRepository _doctorsRepository;
        private readonly IRoomsRepository _roomsRepository;

       
        public AgendaController(
             DataContext context,
            IUserHelper userHelper,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IDoctorsRepository doctorsRepository,
            IRoomsRepository roomsRepository,
            IOwnersRepository ownersRepository,
            
            IAgendaRepository agendaRepository,
            IAnimalsRepository animalsRepository)
        {
            _dataContext = context;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _doctorsRepository = doctorsRepository;
            _ownersRepository = ownersRepository;
            _animalsRepository = animalsRepository;
            
            _roomsRepository = roomsRepository;
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







            //return View(_dataContext.Agendas
            //    .Include(a => a.Owner)
            //    .ThenInclude(o => o.User)
            //    .Include(a => a.Pet)
            //    .Where(a => a.Date >= DateTime.Today.ToUniversalTime()));
        }

        public async Task<IActionResult> AddDays()
        {
            await _agendaRepository.AddDaysAsync(7);
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
               
                Animals = _combosHelper.GetComboAnimals(0),
                Rooms = _combosHelper.GetComboRooms(),
            
                ListServiceTypes = _combosHelper.GetComboAnimalTypes(),

                Doctors = _combosHelper.GetComboDoctor(),
                Owners = _combosHelper.GetComboOwners(),
                
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assing(AgendaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var agenda = await _dataContext.Agendas.FindAsync(model.Id);
                if (agenda != null)
                {
                    agenda.IsAvailable = false;
                    agenda.Owner = await _dataContext.Owners.FindAsync(model.OwnerId);
                    agenda.Animal = await _dataContext.Animals.FindAsync(model.AnimalId);
                    agenda.Remarks = model.Remarks;
                    _dataContext.Agendas.Update(agenda);
                    await _dataContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            model.Owners = _combosHelper.GetComboOwners();
            model.Animals = _combosHelper.GetComboAnimals(model.OwnerId);

            return View(model);
        }

        public async Task<JsonResult> GetAnimalsAsync(int ownerId)
        {
            var pets = await _dataContext.Animals
                .Where(p => p.Owner.Id == ownerId)
                .OrderBy(p => p.Name)
                .ToListAsync();
            return Json(pets);
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
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (agenda == null)
            {
                return NotFound();
            }

            agenda.IsAvailable = true;
            agenda.Animal = null;
            agenda.Owner = null;
            agenda.Remarks = null;

            _dataContext.Agendas.Update(agenda);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
































        //public IActionResult Index()
        //{
        //    return View(_dataContext.Agendas
        //        .Include(a => a.Doctor)
        //        .ThenInclude(s => s.ServiceType)
        //        .Include(s => s.Room)
        //        .Include(a => a.Owner)
        //        .ThenInclude(o => o.User)
        //        .Include(a => a.Animal)
        //        .Where(a => a.Date >= DateTime.Today));


        //}

        //public IActionResult CreateAppointment()
        //{
        //    var model = new AgendaViewModel
        //    {
        //        ListServiceTypes = _agendaRepository.GetComboServiceTypes(),
        //        Rooms = _agendaRepository.GetComboRooms(),
        //        Doctors = _agendaRepository.GetComboDoctors(),
        //        Animals = _agendaRepository.GetComboAnimals(),
        //        Owners = _agendaRepository.GetComboOwners()
        //    };

        //    return View(model);


        //}

        //public /*async Task<*/IActionResult/*>*/ DateList()
        //{
        //    return View(_dataContext.Agendas
        //        .Include(a => a.Doctor)
        //        .ThenInclude(s => s.ServiceType)
        //        .Include(s => s.Room)
        //        .Include(a => a.Owner)
        //        .ThenInclude(o => o.User)
        //        .Include(a => a.Animal)
        //        .Where(a => a.Date >= DateTime.Today));


        //    //await _agendaHelper.SearchDaysAsync(7);
        //    //return RedirectToAction(nameof(DateList));
        //}










        ////[HttpPost]
        ////public async Task<IActionResult> CreateAppointment(DoctorViewModel model)
        ////{


        ////    if (ModelState.IsValid)
        ////    {
        ////        Guid imageId = Guid.Empty;
        ////        if (model.ImageFile != null && model.ImageFile.Length > 0)
        ////        {

        ////            imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "foto");
        ////        }

        ////        var user = new User
        ////        {

        ////            Address = model.Address,
        ////            Document = model.Document,
        ////            Email = model.Username,
        ////            FirstName = model.FirstName,
        ////            LastName = model.LastName,
        ////            PhoneNumber = model.PhoneNumber,
        ////            UserName = model.Username,
        ////            ImageId = imageId
        ////        };

        ////        var response = await _userHelper.AddUserAsync(user, model.Password);
        ////        if (response.Succeeded)
        ////        {
        ////            var userInDB = await _userHelper.GetUserByEmailAsync(model.Username);
        ////            await _userHelper.AddUserToRoleAsync(userInDB, "Worker");
        ////            var doctorServiceType = await _doctorsRepository.AddServiceTypeAsync(model);

        ////            var doctor = new Doctors
        ////            {
        ////                Agendas = new List<Agenda>(),
        ////                ServiceType = doctorServiceType.ServiceType,
        ////                User = userInDB,


        ////                Address = model.Address,
        ////                Document = model.Document,
        ////                FirstName = model.FirstName,
        ////                LastName = model.LastName,
        ////                PhoneNumber = model.PhoneNumber,
        ////                ImageId = imageId
        ////            };

        ////            await _doctorsRepository.CreateAsync(doctor);

        ////            try
        ////            {
        ////                await _context.SaveChangesAsync();



        ////                return RedirectToAction(nameof(Index));
        ////            }
        ////            catch (Exception ex)
        ////            {
        ////                ModelState.AddModelError(string.Empty, ex.ToString());
        ////                return View(model);
        ////            }
        ////        }

        ////        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
        ////    }

        ////    return View(model);
        ////}
















        ////public IActionResult CreateAppointment()
        ////{
        ////    return View(_dataContext.Agendas
        ////        .Include(d => d.Owner)
        ////        .ThenInclude(d => d.User)
        ////        .Include(d => d.Doctor)
        ////        .Include(d => d.Room)
        ////        .Include(d => d.ServiceType)
        ////        );
        ////}

        ////public async Task<IActionResult> SearchDays()
        ////{
        ////    await _agendaHelper.SearchDaysAsync(7);
        ////    return RedirectToAction(nameof(DateList));
        ////}









        ////public async Task<IActionResult> SearchDays(int? id)
        ////{
        ////    if (id == null)
        ////    {
        ////        return NotFound();
        ////    }

        ////    var doctor = await _context.Doctors
        ////        .Include(d => d.User)
        ////        .Include(d => d.ServiceType)
        ////        .FirstOrDefaultAsync(m => m.Id == id);
        ////    if (doctor == null)
        ////    {
        ////        return NotFound();
        ////    }

        ////    return View(doctor);
        ////}















        ////public async Task<IActionResult> AddDays()
        ////{
        ////    await _agendaHelper.AddDaysAsync(7);
        ////    return RedirectToAction(nameof(Index));
        ////}

        //public async Task<IActionResult> Assing(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var agenda = await _dataContext.Agendas
        //        .FirstOrDefaultAsync(o => o.Id == id.Value);
        //    if (agenda == null)
        //    {
        //        return NotFound();
        //    }

        //    var model = new AgendaViewModel
        //    {
        //        Id = agenda.Id,
        //        Owners = _combosHelper.GetComboOwners(),
        //        Animals = _combosHelper.GetComboAnimals(0),
        //        Rooms = _combosHelper.GetComboRooms(),
        //        Doctors = _combosHelper.GetComboDoctor(),
        //    };

        //    return View(model);
        //}

        //[HttpPost]

        //public async Task<IActionResult> Assing(AgendaViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var agenda = await _agendaRepository.GetByIdAsync(model.Id);
        //        if (agenda != null)
        //        {
        //            agenda.IsAvailable = false;
        //            agenda.Owner = await _ownersRepository.GetByIdAsync(model.OwnerId);
        //            agenda.Animal = await _animalsRepository.GetByIdAsync(model.AnimalId);
        //            agenda.Room = await _roomsRepository.GetByIdAsync(model.RoomId);
        //            agenda.Doctor = await _doctorsRepository.GetByIdAsync(model.DoctorId);


        //            agenda.Remarks = model.Remarks;
        //           await _agendaRepository.UpdateAsync(agenda);
        //            //await _dataContext.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }

        //    model.Owners = _combosHelper.GetComboOwners();
        //    model.Animals = _combosHelper.GetComboAnimals(model.OwnerId);
        //    model.Rooms = _combosHelper.GetComboRooms();
        //    model.Doctors = _combosHelper.GetComboDoctor();
        //    return View(model);
        //}

        //public async Task<JsonResult> GetAnimalsAsync(int ownerId)
        //{
        //    var Animals = await _dataContext.Animals
        //        .Where(p => p.Owner.Id == ownerId)
        //        .OrderBy(p => p.Name)
        //        .ToListAsync();
        //    return Json(Animals);
        //}

        //public async Task<IActionResult> Unassign(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var agenda = await _dataContext.Agendas
        //        .Include(a => a.Owner)
        //        .Include(a => a.Animal)
        //        .Include(a => a.Doctor)
        //        .Include(a => a.Room)
        //        .FirstOrDefaultAsync(o => o.Id == id.Value);
        //    if (agenda == null)
        //    {
        //        return NotFound();
        //    }

        //    agenda.IsAvailable = true;
        //    agenda.Animal = null;
        //    agenda.Owner = null;
        //    agenda.Doctor = null;
        //    agenda.Room = null;
        //    agenda.Remarks = null;

        //    await _agendaRepository.UpdateAsync(agenda);
        //    //await _dataContext.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
