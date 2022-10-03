using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VeterinaryClinicGS.Data.Entity;

namespace VeterinaryClinicGS.Data.Entities
{
    public class AnimalType
    {
        public int Id { get; set; }

        [Display(Name = "AnimalType")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Name { get; set; }

        public ICollection<Animals> Animal { get; set; }
    }
}
