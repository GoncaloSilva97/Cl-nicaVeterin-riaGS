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
        private readonly IAgendaHelper _agendaHelper;
        private readonly ICombosHelper _combosHelper;

        public AgendaController(
            DataContext dataContext, 
            IAgendaHelper agendaHelper,
            ICombosHelper combosHelper)
        {
            _dataContext = dataContext;
            _agendaHelper = agendaHelper;
            _combosHelper = combosHelper;
        }

        public IActionResult Index()
        {
            return View(_dataContext.Agendas
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
                Pets = _combosHelper.GetComboPets(0)
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
            model.Pets = _combosHelper.GetComboPets(model.OwnerId);

            return View(model);
        }

        public async Task<JsonResult> GetPetsAsync(int ownerId)
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
    }
}
