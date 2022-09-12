using System.ComponentModel.DataAnnotations;

namespace VeterinaryClinicGS.Data.Entity
{
    public class Appointments : IEntity
    {
        public int Id { get; set; }


        [Display(Name = "AnimalID")]
        public int AnimalID { get; set; }

        [Display(Name = "DoctorID")]
        public int DoctorID { get; set; }

        [Display(Name = "RoomID")]
        public int RoomID { get; set; }
    }
}
