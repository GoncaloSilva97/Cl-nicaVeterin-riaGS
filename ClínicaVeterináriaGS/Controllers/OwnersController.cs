using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeterinaryClinicGS.Data;
using VeterinaryClinicGS.Data.Entity;
using VeterinaryClinicGS.Helperes;

using VeterinaryClinicGS.Models;

namespace VeterinaryClinicGS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OwnersController : Controller
    {
  
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IOwnersRepository _ownersRepository;
        private readonly IAnimalsRepository _animalsRepository;

       

        public OwnersController(
            
            DataContext context,
            IUserHelper userHelper,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IImageHelper imageHelper,
            IBlobHelper blobHelper,
            IOwnersRepository ownersRepository,
            IAnimalsRepository animalsRepository)
        {
            
            _context = context;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
            _blobHelper = blobHelper;
            _ownersRepository = ownersRepository;
            _animalsRepository = animalsRepository;
        }

        public IActionResult Index()
        {
            return View(_context.Owners
                .Include(o => o.User)
                .Include(o => o.Animal));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owners
                .Include(o => o.User)
                .Include(o => o.Animal)
                .ThenInclude(p => p.AnimalType)
                .Include(o => o.Animal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
           

            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {

                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "foto");
                }

                var user = new User
                {
                  
                    Address = model.Address,
                    Document = model.Document,
                    Email = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Username,
                    ImageId = imageId
                };

                var response = await _userHelper.AddUserAsync(user, model.Password);
                if (response.Succeeded)
                {
                    var userInDB = await _userHelper.GetUserByEmailAsync(model.Username);
                    await _userHelper.AddUserToRoleAsync(userInDB, "Customer");

                    var owner = new Owners
                    {
                        Agendas = new List<Agenda>(),
                        Animal = new List<Animals>(),
                        User = userInDB
                    };

                    _context.Owners.Add(owner);

                    try
                    {
                        await _context.SaveChangesAsync();

                       

                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.ToString());
                        return View(model);
                    }
                }

                ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owners
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (owner == null)
            {
                return NotFound();
            }


            var model = new EditUserViewModel
            {
                Address = owner.User.Address,
                Document = owner.User.Document,
                FirstName = owner.User.FirstName,
                Id = owner.Id,
                LastName = owner.User.LastName,
                PhoneNumber = owner.User.PhoneNumber,
                ImageId = owner.User.ImageId,
                Email = owner.User.Email,
           
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                Guid imageId = model.ImageId;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "foto");
                }

                var owner = await _context.Owners
                    .Include(o => o.User)
                    .FirstOrDefaultAsync(o => o.Id == model.Id);

                owner.User.Document = model.Document;
                owner.User.FirstName = model.FirstName;
                owner.User.LastName = model.LastName;
                owner.User.Address = model.Address;
                owner.User.PhoneNumber = model.PhoneNumber;
                owner.User.ImageId = imageId; 

                await _userHelper.UpdateUserAsync(owner.User);
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


            var owner = await _context.Owners
                .Include(o => o.User)
                .Include(o => o.Animal)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (owner == null)
            {
                return NotFound();
            }

            //if (owner.Animal.Count > 0)
            //{
            //    return RedirectToAction(nameof(Index));
            //}

           
           
            return View(owner);
        }



        [HttpPost, ActionName("Delete")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var owner = await _ownersRepository.GetByIdAsync(id);
            try
            {
                await _ownersRepository.DeletAsync(owner);
                //await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{owner.User.FullName} provavelmente está a ser usado!!";
                    ViewBag.ErrorMessage = $"{owner.User.FullName} não pode ser apagado visto haverem encomendas que o usam.</br></br>" +
                        $"Experimente primeiro apagar todas as encomendas que o estão a usar," +
                        $"e torne novamente a apagá-lo";
                }

                return View("Error");
            }
        }


        private bool OwnerExists(int id)
        {
            return _context.Owners.Any(e => e.Id == id);
        }


        public async Task<IActionResult> AddAnimal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _ownersRepository.GetByIdAsync(id.Value);
            if (owner == null)
            {
                return NotFound();
            }

            var model = new AnimalViewModel
            {
                Born = DateTime.Today,
                OwnerId = owner.Id,
                AnimalTypes = _combosHelper.GetComboAnimalTypes()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAnimal(AnimalViewModel model)
        {
            if (ModelState.IsValid)
            {

                Guid imageId = model.ImageId;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "foto");

                }
                
                var Animal = await _converterHelper.ToAnimalAsync(model, imageId, true);
                await _animalsRepository.CreateAsync(Animal);
                //await _context.SaveChangesAsync();
                return RedirectToAction($"Details/{model.OwnerId}");
            }

            model.AnimalTypes = _combosHelper.GetComboAnimalTypes();
            return View(model);
        }



        public async Task<IActionResult> EditAnimal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals
                .Include(p => p.Owner)
                .Include(p => p.AnimalType)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(_converterHelper.ToAnimalViewModel(animal));
        }

        [HttpPost]
        public async Task<IActionResult> EditAnimal(AnimalViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = model.ImageId;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "foto");

                }

                

                var animal = await _converterHelper.ToAnimalAsync(model, imageId, false);
                await _animalsRepository.UpdateAsync(animal);
                //await _context.SaveChangesAsync();
                return RedirectToAction($"Details/{model.OwnerId}");
            }

            model.AnimalTypes = _combosHelper.GetComboAnimalTypes();
            return View(model);
        }


        public async Task<IActionResult> DetailsAnimal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals
                .Include(p => p.Owner)
                .ThenInclude(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }





        public async Task<IActionResult> DeleteAnimal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.Id == id.Value);
            if (animal == null)
            {
                return NotFound();
            }



            await _animalsRepository.DeletAsync(animal);
            //await _context.SaveChangesAsync();
            return RedirectToAction($"{nameof(Details)}/{animal.Owner.Id}");
        }
    }
}
