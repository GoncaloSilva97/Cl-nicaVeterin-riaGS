using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VeterinaryClinicGS.Data.Entity;

namespace VeterinaryClinicGS.Data.Entity
{
    public class Doctors : IEntity
    {
        public int Id { get; set; }


        public User User { get; set; }


        public ServiceTypes ServiceType { get; set; }

        public ICollection<Agenda> Agendas { get; set; }












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

        public string PhoneNumber { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
         ? $"https://veterinaryclinicgsstorag.azurewebsites.net/foto/noimage.png"
         : $"https://veterinaryclinicgsstorag.blob.core.windows.net/foto/{ImageId}";









        [Required]
        [Display(Name = "Service Type")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Service Type.")]
        public int ServiceTypeId { get; set; }
    }
}
//DoctorsController
//DoctorsViewModel