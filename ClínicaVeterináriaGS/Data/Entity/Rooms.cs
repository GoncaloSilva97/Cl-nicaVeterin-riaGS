using System.ComponentModel.DataAnnotations;

namespace VeterinaryClinicGS.Data.Entity
{
    public class Rooms : IEntity
    {
        public int Id { get; set; }


        [Display(Name = "Specialty")]
        public string Specialty { get; set; }

        
    }
}
