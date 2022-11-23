using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VeterinaryClinicGS.Data.Entity;
using VeterinaryClinicGS.Helperes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace VeterinaryClinicGS.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        private Random _random;
        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Customer");
            await _userHelper.CheckRoleAsync("Worker");

            var user = await _userHelper.GetUserByEmailAsync("dalton.fury120@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Gonçalo",
                    LastName = "Silva",
                    Email = "dalton.fury120@gmail.com",
                    UserName = "dalton.fury120@gmail.com",
                    PhoneNumber = "123456789",
                    Address = "dalton.fury120@gmail.com_123456789",
                    Document = "123456789",                   
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");



                //var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                //await _userHelper.ConfirmEmailAsync(user, token);



            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }
        }
    }   
}
