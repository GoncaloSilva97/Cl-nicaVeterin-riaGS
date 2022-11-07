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
        [Display(Name = "Animal")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select an animal.")]
        public int AnimalId { get; set; }

        public IEnumerable<SelectListItem> Animals { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Doctor")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a doctor.")]
        public int DoctorId { get; set; }

        public IEnumerable<SelectListItem> Doctors { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Room")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a room.")]
        public int RoomId { get; set; }

        public IEnumerable<SelectListItem> Rooms { get; set; }

        public bool IsMine { get; set; }

        public string Reserved => "Reserved";
    }
}
