using System;
using System.ComponentModel.DataAnnotations;

namespace VeterinaryClinicGS.Data.Entity
{
    public class Doctors : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; } 

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Specialty")]
        public string Specialty { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Display(Name = "Appointment List")]
        //public string Species { get; set; }


        public string ImageFullPath => ImageId == Guid.Empty
          ? $"https://veterinaryclinicgs.azurewebsites.net/foto/noimage.png"
          : $"https://veterinaryclinicgs.blob.core.windows.net/foto/{ImageId}";
    }
}
