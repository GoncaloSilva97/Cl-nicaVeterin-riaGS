using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace VeterinaryClinicGS.Data.Entity
{
    public class User : IdentityUser
    {
     

        [Display(Name = "Document")]
        [MaxLength(20, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Document { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string LastName { get; set; }

        [MaxLength(100, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Address { get; set; }



        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";



        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
         ? $"https://veterinaryclinicgs.azurewebsites.net/foto/noimage.png"
         : $"https://veterinaryclinicgs.blob.core.windows.net/foto/{ImageId}";













        //[Display(Name = "Email")]
        //[Required(ErrorMessage = "The field {0} is mandatory.")]
        //[MaxLength(100, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        //[EmailAddress]
        //public string Username { get; set; }

        //[Display(Name = "Password")]
        //[Required(ErrorMessage = "The field {0} is mandatory.")]
        //[DataType(DataType.Password)]
        //[StringLength(20, MinimumLength = 6, ErrorMessage = "The {0} field must contain between {2} and {1} characters.")]
        //public string Password { get; set; }

        //[Display(Name = "Password Confirm")]
        //[Required(ErrorMessage = "The field {0} is mandatory.")]
        //[DataType(DataType.Password)]
        //[StringLength(20, MinimumLength = 6, ErrorMessage = "The {0} field must contain between {2} and {1} characters.")]
        //[Compare("Password")]
        //public string PasswordConfirm { get; set; }
    }
}
