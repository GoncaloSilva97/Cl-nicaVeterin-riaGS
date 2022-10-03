using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using VeterinaryClinicGS.Data.Entity;

namespace VeterinaryClinicGS.Models
{
    public class OwnerViewModel : Owners
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

    }
}
