using System;
using System.IO;
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
    //[Authorize(Roles = "Admin")]
    public class AnimalsController : Controller
    {
        private readonly ICombosHelper _combosHelper;
        private readonly DataContext _dataContext;
        private readonly IBlobHelper _blobHelper;
        private readonly IAnimalsRepository _animalsRepository;


        public AnimalsController(
            ICombosHelper combosHelper,
            DataContext dataContext,
            IBlobHelper blobHelper,
            IAnimalsRepository animalsRepository)
        {
            _combosHelper = combosHelper;
            _dataContext = dataContext;
            _blobHelper = blobHelper;
            _animalsRepository = animalsRepository;
        }

        public IActionResult Index()
        {
            return View(_dataContext.Animals
                .Include(p => p.Owner)
                .ThenInclude(o => o.User)
                .Include(p => p.AnimalType));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animals = await _dataContext.Animals
                .Include(p => p.Owner)
                .ThenInclude(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (animals == null)
            {
                return NotFound();
            }

            return View(animals);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _dataContext.Animals
                .Include(p => p.Owner)
                .Include(p => p.AnimalType)
                .FirstOrDefaultAsync(p => p.Id == id.Value);
            if (animal == null)
            {
                return NotFound();
            }

            var view = new AnimalViewModel
            {
                Born = animal.Born,
                Id = animal.Id,
                ImageId = animal.ImageId,
                Name = animal.Name,
                OwnerId = animal.Owner.Id,
                AnimalTypeId = animal.AnimalType.Id,
                AnimalTypes = _combosHelper.GetComboAnimalTypes(),
                Race = animal.Race,
                Remarks = animal.Remarks
            };

            return View(view);
        }

        [HttpPost]
       
        public async Task<IActionResult> Edit(AnimalViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = model.ImageId;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "foto");

                }
                
                var animal = new Animals
                {
                    Born = model.Born,
                    Id = model.Id,
                    ImageId = imageId,
                    Name = model.Name,
                    Owner = await _dataContext.Owners.FindAsync(model.OwnerId),
                    AnimalType = await _dataContext.AnimalTypes.FindAsync(model.AnimalTypeId),
                    Race = model.Race,
                    Remarks = model.Remarks
                };

                await _animalsRepository.UpdateAsync(animal);
                //await _dataContext.SaveChangesAsync();
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

            var animals = await _dataContext.Animals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animals == null)
            {
                return NotFound();
            }

            await _animalsRepository.DeletAsync(animals);
            //await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       
        

        
    }
}
