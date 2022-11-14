
using System;
using System.ComponentModel.DataAnnotations;


namespace VeterinaryClinicGS.Data.Entity
{
    public class Agenda : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Date")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public string Remarks { get; set; }

        [Display(Name = "Is Available?")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public DateTime DateLocal => Date.ToLocalTime();

        public Owners Owner { get; set; }

        public Animals Animal { get; set; }

        public Doctors Doctor { get; set; }

        public Rooms Room { get; set; }

        public ServiceTypes ServiceType { get; set; }
    }
}
