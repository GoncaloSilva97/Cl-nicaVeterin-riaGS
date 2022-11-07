using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VeterinaryClinicGS.Data.Entity
{
    public class Rooms : IEntity
    {
        public int Id { get; set; }


        public int RoomNumber { get; set; }
        public ServiceTypes ServiceType { get; set; }

        public ICollection<Agenda> Agendas { get; set; }

        [Required]
        [Display(Name = "Service Type")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Service Type.")]
        public int ServiceTypeId { get; set; }
    }
}
//RoomsController
//RoomsViewModel