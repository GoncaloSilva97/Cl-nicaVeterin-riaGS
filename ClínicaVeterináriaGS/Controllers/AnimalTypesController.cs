using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VeterinaryClinicGS.Data;
using VeterinaryClinicGS.Data.Entity;
using VeterinaryClinicGS.Helperes;
using VeterinaryClinicGS.Models;

namespace VeterinaryClinicGS.Controllers
{
    public class AnimalTypesController : Controller
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
        private readonly IAnimalTypesRepository _animalTypesRepository;

     

        public AnimalTypesController(
             DataContext context,
            IUserHelper userHelper,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IImageHelper imageHelper,
            IBlobHelper blobHelper,
            IOwnersRepository ownersRepository,
           
            IAgendaRepository agendaRepository,
            IAnimalsRepository animalsRepository,
            IServiceTypesRepository serviceTypesRepository,
            IAnimalTypesRepository animalTypesRepository)
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
            _animalTypesRepository = animalTypesRepository;
        }




        // GET: PetTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.AnimalTypes.ToListAsync());
        }

        // GET: PetTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animalType = await _animalTypesRepository.GetByIdAsync(id.Value);
            if (animalType == null)
            {
                return NotFound();
            }

            return View(animalType);
        }

        // GET: PetTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PetTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(AddAnimalTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _animalTypesRepository.CreateAsync(model);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: PetTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animalType = await _animalTypesRepository.GetByIdAsync(id.Value);
            if (animalType == null)
            {
                return NotFound();
            }
            return View(animalType);
        }

        // POST: PetTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost] 
        public async Task<IActionResult> Edit(int id, AddAnimalTypeViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _animalTypesRepository.UpdateAsync(model);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalTypeExists(model.Id))
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

            var animalType = await _context.AnimalTypes
                .Include(pt => pt.Animal)
                .FirstOrDefaultAsync(pt => pt.Id == id);
            if (animalType == null)
            {
                return NotFound();
            }

            if (animalType.Animal.Count > 0)
            {
                ViewBag.ErrorTitle = $"{animalType.Name} provavelmente está a ser usado!!";
                ViewBag.ErrorMessage = $"{animalType.Name} não pode ser apagado visto haverem animais deste tipo.</br></br>" +
                    $"Experimente primeiro apagar todos os animais dete tipo," +
                    $"e torne novamente a apagá-lo";

                return View("Error");
            }

            await _animalTypesRepository.DeletAsync(animalType);
            return RedirectToAction(nameof(Index));






            
        }

        private bool AnimalTypeExists(int id)
        {
            return _context.AnimalTypes.Any(e => e.Id == id);
        }
    }
}
