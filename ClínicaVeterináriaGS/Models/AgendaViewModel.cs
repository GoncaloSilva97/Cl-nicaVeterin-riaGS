using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using VeterinaryClinicGS.Data.Entity;


namespace VeterinaryClinicGS.Models
{
    public class AgendaViewModel : Agenda
    {
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Owner")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select an owner.")]
        public int OwnerId { get; set; }

        public IEnumerable<SelectListItem> Owners { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Pet")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a pet.")]
        public int AnimalId { get; set; }

        public IEnumerable<SelectListItem> Pets { get; set; }

        public bool IsMine { get; set; }

        public string Reserved => "Reserved";
    }
}
