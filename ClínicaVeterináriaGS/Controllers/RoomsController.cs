using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VeterinaryClinicGS.Data;
using VeterinaryClinicGS.Data.Entity;
using VeterinaryClinicGS.Helperes;
using VeterinaryClinicGS.Models;

namespace VeterinaryClinicGS.Controllers
{
    public class RoomsController : Controller
    {
        private readonly DataContext _context; 
        private readonly IRoomsRepository _roomsRepository; 
        private readonly IServiceTypesRepository _serviceTypesRepository;

        public RoomsController(
             DataContext context,              
            IRoomsRepository roomsRepository,
            IServiceTypesRepository serviceTypesRepository)
        {
            _context = context;            
            _roomsRepository = roomsRepository;
            _serviceTypesRepository = serviceTypesRepository;
        }




        // GET: ServiceTypes
        public IActionResult Index()
        {
            return View(_context.Rooms
                .Include(d => d.ServiceType));

        }



        // GET: ServiceTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(d => d.ServiceType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }



        // GET: ServiceTypes/Create
        public IActionResult Create()
        {
            var model = new RoomsViewModel
            {
                ListServiceTypes = _roomsRepository.GetComboServiceTypes()
            };

            return View(model);
        }

        // POST: ServiceTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
   
        public async Task<IActionResult> Create(RoomsViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _roomsRepository.CreateAsync(model);
       
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        // GET: ServiceTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(o => o.ServiceType)
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (room == null)
            {
                return NotFound();
            }

            

            var model = new RoomsViewModel
            {
                
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                ServiceType = room.ServiceType,
                
                ListServiceTypes = _roomsRepository.GetComboServiceTypes(),
                ServiceTypeId = room.ServiceTypeId
            };

            return View(model);

        }

        // POST: ServiceTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
    
        public async Task<IActionResult> Edit(RoomsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _roomsRepository.UpdateAsync(model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomsExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }




        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
               .Include(o => o.ServiceType)
               .FirstOrDefaultAsync(o => o.Id == id);
            if (room == null)
            {
                return NotFound();
            }

           
            return View(room);
        }



        [HttpPost, ActionName("Delete")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _roomsRepository.GetByIdAsync(id);
            try
            {
                await _roomsRepository.DeletAsync(room);
                //await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"A sala {room.Id} provavelmente está a ser usada!!";
                    ViewBag.ErrorMessage = $"A sala {room.Id} não pode ser apagada visto haverem consultas que a usam.</br></br>" +
                        $"Experimente primeiro apagar todas as consultas que o estão a usar," +
                        $"e torne novamente a apagá-lo";
                }

                return View("Error");
            }
        }






        private bool RoomsExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
    }
}
