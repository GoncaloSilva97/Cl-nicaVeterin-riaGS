using System;
using System.IO;
using System.Threading.Tasks;
using ClínicaVeterináriaGS.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeterinaryClinicGS.Data;
using VeterinaryClinicGS.Data.Entity;
using VeterinaryClinicGS.Helperes;
using VeterinaryClinicGS.Helpers;
using VeterinaryClinicGS.Models;

namespace VeterinaryClinicGS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AnimalsController : Controller
    {
        private readonly ICombosHelper _combosHelper;
        private readonly DataContext _dataContext;
        private readonly BlobHelper _blobHelper;

        public AnimalsController(
            ICombosHelper combosHelper,
            DataContext dataContext,
            BlobHelper blobHelper)
        {
            _combosHelper = combosHelper;
            _dataContext = dataContext;
            _blobHelper = blobHelper;
        }

        public IActionResult Index()
        {
            return View(_dataContext.Animals
                .Include(p => p.Owner)
                .ThenInclude(o => o.User)
                .Include(p => p.AnimalType)
                .Include(p => p.Histories));
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
                .Include(p => p.Histories)
                .ThenInclude(h => h.ServiceType)
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
                AnimalTypes = _combosHelper.GetComboPetTypes(),
                Race = animal.Race,
                Remarks = animal.Remarks
            };

            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

                _dataContext.Animals.Update(animal);
                await _dataContext.SaveChangesAsync();
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

            _dataContext.Animals.Remove(animals);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteHistory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var history = await _dataContext.Histories
                .Include(h => h.Animal)
                .FirstOrDefaultAsync(h => h.Id == id.Value);
            if (history == null)
            {
                return NotFound();
            }

            _dataContext.Histories.Remove(history);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction($"{nameof(Details)}/{history.Animal.Id}");
        }

        public async Task<IActionResult> EditHistory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var history = await _dataContext.Histories
                .Include(h => h.Animal)
                .Include(h => h.ServiceType)
                .FirstOrDefaultAsync(p => p.Id == id.Value);
            if (history == null)
            {
                return NotFound();
            }

            var model = new HistoryViewModel
            {
                Date = history.Date,
                Description = history.Description,
                Id = history.Id,
                AnimalId = history.Animal.Id,
                Remarks = history.Remarks,
                ServiceTypeId = history.ServiceType.Id,
                ServiceTypes = _combosHelper.GetComboServiceTypes()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditHistory(HistoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var history = new History
                {
                    Date = model.Date,
                    Description = model.Description,
                    Id = model.Id,
                    Animal = await _dataContext.Animals.FindAsync(model.AnimalId),
                    Remarks = model.Remarks,
                    ServiceType = await _dataContext.ServiceTypes.FindAsync(model.ServiceTypeId)
                };

                _dataContext.Histories.Update(history);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(Details)}/{model.AnimalId}");
            }

            return View(model);
        }

        public async Task<IActionResult> AddHistory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _dataContext.Animals.FindAsync(id.Value);
            if (animal == null)
            {
                return NotFound();
            }

            var model = new HistoryViewModel
            {
                Date = DateTime.Now,
                AnimalId = animal.Id,
                ServiceTypes = _combosHelper.GetComboServiceTypes(),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddHistory(HistoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var history = new History
                {
                    Date = model.Date,
                    Description = model.Description,
                    Animal = await _dataContext.Animals.FindAsync(model.AnimalId),
                    Remarks = model.Remarks,
                    ServiceType = await _dataContext.ServiceTypes.FindAsync(model.ServiceTypeId)
                };

                _dataContext.Histories.Add(history);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(Details)}/{model.AnimalId}");
            }

            return View(model);
        }
    }
}
