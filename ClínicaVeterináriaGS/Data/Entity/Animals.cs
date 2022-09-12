using System.ComponentModel.DataAnnotations;

namespace VeterinaryClinicGS.Data.Entity
{
    public class Animals : IEntity
    {
        public int Id { get; set; }


        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Chip")]
        public string Chip { get; set; }

        [Display(Name = "Species")]
        public string Species { get; set; }
    }
}
