using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinicGS.Data;
using VeterinaryClinicGS.Data.Entity;
using VeterinaryClinicGS.Helperes;
using VeterinaryClinicGS.Models;

namespace VeterinaryClinicGS.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IDoctorsRepository _doctorsRepository;
        private readonly IMailHelper _mailHelper;


        public IConfiguration _configuration { get; }
        public DoctorsController(

            DataContext context,
            IUserHelper userHelper,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IImageHelper imageHelper,
            IBlobHelper blobHelper,
            IMailHelper mailHelper,
            IDoctorsRepository doctorsRepository,
            IConfiguration configuration)
        {
            _mailHelper = mailHelper;
            _context = context;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
            _blobHelper = blobHelper;
            _doctorsRepository = doctorsRepository;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View(_context.Doctors
                .Include(d => d.User)
                .Include(d => d.ServiceType));
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.ServiceType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        public IActionResult Create()
        {
            var model = new DoctorViewModel
            {
                ListServiceTypes = _doctorsRepository.GetComboServiceTypes()
            };

            return View(model);

            
        }

        [HttpPost]
        public async Task<IActionResult> Create(DoctorViewModel model)
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
                    await _userHelper.AddUserToRoleAsync(userInDB, "Worker");
                    var doctorServiceType = await _doctorsRepository.AddServiceTypeAsync(model);

                    var doctor = new Doctors
                    {
                        //Agendas = new List<Agenda>(),
                        ServiceType = doctorServiceType.ServiceType,
                        User = userInDB,


                        Address = model.Address,
                        Document = model.Document,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        ImageId = imageId
                    };

                    await _doctorsRepository.CreateAsync(doctor);

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






        //{
        //    if (ModelState.IsValid)
        //    {
        //        //var user = await _userHelper.GetUserByEmailAsync(model.Username);
        //        //if (user == null)
        //        //{
        //        //    Guid imageId = Guid.Empty;
        //        //    if (model.ImageFile != null && model.ImageFile.Length > 0)
        //        //    {

        //        //        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "foto");
        //        //    }

        //        //    user = new User
        //        //    {

        //        //        Address = model.Address,
        //        //        Document = model.Document,
        //        //        Email = model.Username,
        //        //        FirstName = model.FirstName,
        //        //        LastName = model.LastName,
        //        //        PhoneNumber = model.PhoneNumber,
        //        //        UserName = model.Username,
        //        //        ImageId = imageId
        //        //    };


        //        //    var result = await _userHelper.AddUserAsync(user, model.Password);
        //        //    if (result != IdentityResult.Success)
        //        //    {
        //        //        ModelState.AddModelError(string.Empty, "The user couldn't be created.");
        //        //        return View(model);
        //        //    }

        //        //    await _userHelper.AddUserToRoleAsync(user, "Worker");


        //        //    var isInRole = await _userHelper.IsUserInRoleAsync(user, "Worker");
        //        //    if (!isInRole)
        //        //    {
        //        //        await _userHelper.AddUserToRoleAsync(user, "Worker");
        //        //    }
        //        //    var doctorServiceType = await _doctorsRepository.AddServiceTypeAsync(model);

        //        //    var doctor = new Doctors
        //        //    {
        //        //        Agendas = new List<Agenda>(),
        //        //        ServiceType = doctorServiceType.ServiceType,
        //        //        User = user,


        //        //        Address = model.Address,
        //        //        Document = model.Document,
        //        //        FirstName = model.FirstName,
        //        //        LastName = model.LastName,
        //        //        PhoneNumber = model.PhoneNumber,
        //        //        ImageId = imageId
        //        //    };

        //        //    await _doctorsRepository.CreateAsync(doctor);
        //        //    //await _context.SaveChangesAsync();





        //        //    string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
        //        //    string tokenLink = Url.Action("ConfirmEmail", "Account", new
        //        //    {
        //        //        userid = user.Id,
        //        //        token = myToken
        //        //    }, protocol: HttpContext.Request.Scheme);

        //        //    Response response = _mailHelper.SendEmail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
        //        //        $"To allow the user, " +
        //        //        $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");


        //        //    if (response.IsSuccess)
        //        //    {
        //        //        ViewBag.Message = "The instructions to allow you user has been sent to email";
        //        //        return View(model);
        //        //    }

        //        //    ModelState.AddModelError(string.Empty, "The user couldn't be logged.");


        //        //}

















        //        if (ModelState.IsValid)
        //        {
        //            Guid imageId = Guid.Empty;
        //            if (model.ImageFile != null && model.ImageFile.Length > 0)
        //            {

        //                imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "foto");
        //            }

        //            var user = new User
        //            {

        //                Address = model.Address,
        //                Document = model.Document,
        //                Email = model.Username,
        //                FirstName = model.FirstName,
        //                LastName = model.LastName,
        //                PhoneNumber = model.PhoneNumber,
        //                UserName = model.Username,
        //                ImageId = imageId
        //            };

        //            var response = await _userHelper.AddUserAsync(user, model.Password);
        //        if (response.Succeeded)
        //        {
        //            var userInDB = await _userHelper.GetUserByEmailAsync(model.Username);
        //            await _userHelper.AddUserToRoleAsync(userInDB, "Worker");






        //                var doctorServiceType = await _doctorsRepository.AddServiceTypeAsync(model);

        //                var doctor = new Doctors
        //            {
        //                Agendas = new List<Agenda>(),
        //                ServiceType = doctorServiceType.ServiceType,
        //                User = userInDB,


        //                Address = model.Address,
        //                Document = model.Document,
        //                FirstName = model.FirstName,
        //                LastName = model.LastName,
        //                PhoneNumber = model.PhoneNumber,   
        //                ImageId = imageId
        //            };

        //            await _doctorsRepository.CreateAsync(doctor);

        //            try
        //            {
        //                await _context.SaveChangesAsync();


        //                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
        //                string tokenLink = Url.Action("ConfirmEmail", "Account", new
        //                {
        //                    userid = user.Id,
        //                    token = myToken
        //                }, protocol: HttpContext.Request.Scheme);

        //                Response response2 = _mailHelper.SendEmail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
        //                    $"To allow the user, " +
        //                    $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");


        //                if (response2.IsSuccess)
        //                {
        //                    ViewBag.Message = "The instructions to allow you user has been sent to email";
        //                    return View(model);
        //                }

        //                ModelState.AddModelError(string.Empty, "The user couldn't be logged.");







                        



        //                return RedirectToAction(nameof(Index));
        //            }
        //            catch (Exception ex)
        //            {
        //                ModelState.AddModelError(string.Empty, ex.ToString());
        //                return View(model);
        //            }
        //        }



               
        //    }

        //    return View(model);
        //}


        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return this.Created(string.Empty, results);

                    }
                }
            }

            return BadRequest();
        }



























        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {

            }

            return View();

        }
































        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(o => o.User)
                .Include(o => o.ServiceType)
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (doctor == null)
            {
                return NotFound();
            }

            //var user = await _userHelper.get(model.Username);

            var model = new EditDoctorViewModel
            {
                Address = doctor.User.Address,
                Document = doctor.User.Document,
                FirstName = doctor.User.FirstName,
                Id = doctor.Id,
                LastName = doctor.User.LastName,
                PhoneNumber = doctor.User.PhoneNumber,
                ImageId = doctor.ImageId,
                User = doctor.User,
                ListServiceTypes = _doctorsRepository.GetComboServiceTypes(),
                ServiceTypeId = doctor.ServiceTypeId
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EditDoctorViewModel model)
        {

            if (ModelState.IsValid)
            {
                Guid imageId = model.ImageId;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "foto");
                }
               
                var doctorServiceType = await _doctorsRepository.EditServiceTypeAsync(model);
                var doctor = await _context.Doctors
                    .Include(o => o.User)
                    .FirstOrDefaultAsync(o => o.Id == model.Id);




                doctor.User.Document = model.Document;
                doctor.User.FirstName = model.FirstName;
                doctor.User.LastName = model.LastName;
                doctor.User.Address = model.Address;
                doctor.User.PhoneNumber = model.PhoneNumber;
                doctor.User.ImageId = imageId;
                doctor.ServiceType = doctorServiceType.ServiceType;
                doctor.ServiceTypeId = doctor.ServiceTypeId;
                doctor.User = doctor.User;
                
                await _userHelper.UpdateUserAsync(doctor.User);
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


            var doctor = await _context.Doctors
                .Include(o => o.User)             
                .FirstOrDefaultAsync(o => o.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }



        [HttpPost, ActionName("Delete")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _doctorsRepository.GetByIdAsync(id);
            try
            {
                await _doctorsRepository.DeletAsync(doctor);
                //await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{doctor.Name} provavelmente está a ser usado!!";
                    ViewBag.ErrorMessage = $"{doctor.Name} não pode ser apagado visto haverem encomendas que o usam.</br></br>" +
                        $"Experimente primeiro apagar todas as encomendas que o estão a usar," +
                        $"e torne novamente a apagá-lo";
                }

                return View("Error");
            }
        }


        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.Id == id);
        }
    }
}
