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
    }
}
