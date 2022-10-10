
using ClínicaVeterináriaGS.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VeterinaryClinicGS.Data.Entity;

namespace VeterinaryClinicGS.Data.Entity
{
    public class Animals : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Chip")]
        public string Chip { get; set; }

        [Display(Name = "Species")]
        public string Species { get; set; }






        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Race { get; set; }

        [Display(Name = "Born")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime Born { get; set; }

        public string Remarks { get; set; }

        

        [Display(Name = "Born")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime BornLocal => Born.ToLocalTime();

        public AnimalType AnimalType { get; set; }

        public Owners Owner { get; set; }

        public ICollection<History> Histories { get; set; }

        public ICollection<Agenda> Agendas { get; set; }
















        public string ImageFullPath => ImageId == Guid.Empty
           ? $"https://veterinaryclinicgs.azurewebsites.net/foto/noimage.png"
           : $"https://veterinaryclinicgs.blob.core.windows.net/foto/{ImageId}";
    }
}
