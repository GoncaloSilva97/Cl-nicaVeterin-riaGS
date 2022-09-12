using System.ComponentModel.DataAnnotations;

namespace VeterinaryClinicGS.Data.Entity
{
    public class Owners : IEntity
    {
        public int Id { get; set; }


        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "NIF")]
        public string NIF { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Display(Name = "Pet List")]
        //public string Species { get; set; }
    }
   
}
