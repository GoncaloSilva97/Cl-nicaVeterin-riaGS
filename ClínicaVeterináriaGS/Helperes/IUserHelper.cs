﻿using Microsoft.AspNetCore.Identity;
using VeterinaryClinicGS.Data.Entity;
using System.Threading.Tasks;
using VeterinaryClinicGS.Models;

namespace VeterinaryClinicGS.Helperes
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);


        Task<IdentityResult> AddUserAsync(User user, string password);


        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();


        Task<IdentityResult> UpdateUserAsync(User user);


        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);





        Task<bool> DeletAsync(string email);





        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        Task <bool> IsUserInRoleAsync(User user, string roleName);


        Task<SignInResult> ValidatePasswordAsync(User user, string password);


        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        Task<User> GetUserByIdAsync(string userId);
    }
}
