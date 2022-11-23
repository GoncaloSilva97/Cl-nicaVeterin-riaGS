using ClínicaVeterináriaGS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinicGS.Data;

namespace VeterinaryClinicGS.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;
       
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _context = context;
           
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Services() 
        {
            return View(await _context.ServiceTypes.ToListAsync());
        }

        public IActionResult Doctors()
        {
            return View(_context.Doctors
                 .Include(d => d.User)
                 .Include(d => d.ServiceType));
        }

        public IActionResult Schedule()
        {
            return View(_context.Agendas
                .Include(a => a.Doctor)
                .ThenInclude(s => s.ServiceType)
                .Include(s => s.Room)
                .Include(a => a.Owner)
                .ThenInclude(o => o.User)
                .Include(a => a.Animal)
                .Where(a => a.Date >= DateTime.Today));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
