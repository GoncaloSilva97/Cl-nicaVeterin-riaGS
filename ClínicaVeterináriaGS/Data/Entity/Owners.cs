using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VeterinaryClinicGS.Data.Entity;

namespace VeterinaryClinicGS.Data.Entity
{
    public class Owners 
    {
        public int Id { get; set; }


        public User User { get; set; }

        public ICollection<Animals> Animal { get; set; }

        public ICollection<Agenda> Agendas { get; set; }






    }

}
