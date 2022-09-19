using Microsoft.AspNetCore.Identity;

namespace VeterinaryClinicGS.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }



    }
}
