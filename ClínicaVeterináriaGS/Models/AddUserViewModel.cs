using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VeterinaryClinicGS.Data.Entity;

namespace VeterinaryClinicGS.Models
{
    public class AddUserViewModel 
    {
       

        [Key]
        public int Id { get; set; }

        [Display(Name = "Document")]
        [MaxLength(20, ErrorMessage = "The {0} field can not have more than {1} characters.")]
       
        public string Document { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
       
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        
        public string LastName { get; set; }

        [MaxLength(100, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";



        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
          ? $"https://veterinaryclinicgsblob.azurewebsites.net/foto/noimage.png"
         : $"https://veterinaryclinicgsblob.blob.core.windows.net/foto/{ImageId}";




        //[Display(Name = "Image")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public IFormFile ImageFile { get; set; }




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



















        [Display(Name = "Service Type")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Service Type.")]
        public ServiceTypes ServiceType { get; set; }



        //[Required]
        [Display(Name = "Service Type")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Service Type.")]
        public int ServiceTypeId { get; set; }

        public IEnumerable<SelectListItem> ListServiceTypes { get; set; }
    }
}
