using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VeterinaryClinicGS.Data.Entities;

namespace VeterinaryClinicGS.Data.Entity
{
    public class Owners : IEntity
    {
        public int Id { get; set; }


        public User User { get; set; }

        public ICollection<Animals> Animal { get; set; }

        public ICollection<Agenda> Agendas { get; set; }







    }
   
}
