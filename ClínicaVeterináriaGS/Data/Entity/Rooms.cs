using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VeterinaryClinicGS.Data.Entity
{
    public class Rooms : IEntity
    {
        public int Id { get; set; }


        
        public ServiceTypes ServiceType { get; set; }

        public ICollection<Agenda> Agendas { get; set; }
    }
}
