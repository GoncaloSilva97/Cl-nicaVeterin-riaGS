using Microsoft.AspNetCore.Mvc;
using VeterinaryClinicGS.Models;
using System.Diagnostics;
using ClínicaVeterináriaGS.Models;

namespace VeterinaryClinicGS.Controllers
{
    public class ErrorsController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[Route("error")]
        //public IActionResult Error()
        //{
        //    return View();
        //}
    }
}
