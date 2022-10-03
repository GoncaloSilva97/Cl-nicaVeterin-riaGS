using Microsoft.AspNetCore.Identity;
using VeterinaryClinicGS.Data.Entities;
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
    }
}
