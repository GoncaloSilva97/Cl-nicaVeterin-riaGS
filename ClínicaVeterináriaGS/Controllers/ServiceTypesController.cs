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
    public class ServiceTypesController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IOwnersRepository _ownersRepository;
        private readonly IAnimalsRepository _animalsRepository;
       
        private readonly IAgendaRepository _agendaRepository;
        private readonly IServiceTypesRepository _serviceTypesRepository;

        

        public ServiceTypesController(
             DataContext context,
            IUserHelper userHelper,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IImageHelper imageHelper,
            IBlobHelper blobHelper,
            IOwnersRepository ownersRepository,
         
            IAgendaRepository agendaRepository,
            IAnimalsRepository animalsRepository,
            IServiceTypesRepository serviceTypesRepository)
        {
            _context = context;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
            _blobHelper = blobHelper;
            _ownersRepository = ownersRepository;
            _animalsRepository = animalsRepository;
         
            _agendaRepository = agendaRepository;
            _serviceTypesRepository = serviceTypesRepository;
        }




        // GET: ServiceTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ServiceTypes.ToListAsync());
        }



        // GET: ServiceTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceType = await _serviceTypesRepository.GetByIdAsync(id.Value);
            if (serviceType == null)
            {
                return NotFound();
            }

            return View(serviceType);
        }



        // GET: ServiceTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
   
        public async Task<IActionResult> Create(ServiceTypesViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _serviceTypesRepository.CreateAsync(model);
                //await _context.SaveChangesAsync();
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

            var serviceType = await _serviceTypesRepository.GetByIdAsync(id.Value);
            if (serviceType == null)
            {
                return NotFound();
            }
            return View(serviceType);
        }

        // POST: ServiceTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
    
        public async Task<IActionResult> Edit(ServiceTypesViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceTypesRepository.UpdateAsync(model);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceTypeExists(model.Id))
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

            var serviceType = await _serviceTypesRepository.GetByIdAsync(id.Value);
            if (serviceType == null)
            {
                return NotFound();
            }

           
            return View(serviceType);
        }



        [HttpPost, ActionName("Delete")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceType = await _serviceTypesRepository.GetByIdAsync(id);
            try
            {
                await _serviceTypesRepository.DeletAsync(serviceType);
                //await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{serviceType.Name} provavelmente está a ser usado!!";
                    ViewBag.ErrorMessage = $"{serviceType.Name} não pode ser apagado visto haverem encomendas que o usam.</br></br>" +
                        $"Experimente primeiro apagar todas as encomendas que o estão a usar," +
                        $"e torne novamente a apagá-lo";
                }

                return View("Error");
            }
        }






        private bool ServiceTypeExists(int id)
        {
            return _context.ServiceTypes.Any(e => e.Id == id);
        }
    }
}
