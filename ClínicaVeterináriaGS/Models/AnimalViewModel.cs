using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

using VeterinaryClinicGS.Data.Entity;

namespace VeterinaryClinicGS.Models
{
    public class AnimalViewModel : Animals
    {
        public int OwnerId { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Animal Type")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Animal type.")]
        public int AnimalTypeId { get; set; }

        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        public IEnumerable<SelectListItem> AnimalTypes { get; set; }
    }
}
