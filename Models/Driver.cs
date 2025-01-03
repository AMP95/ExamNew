using Models.Sub;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Driver
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }                                                                                                                 
        public string FamilyName { get; set; }
        public string FatherName { get; set; }

        public DateTime DateOfBirth { get; set; }


        [ForeignKey(nameof(Passport))]
        public Guid PaddportId { get; set; }
        public virtual Passport Passport { get; set; }

       

        [ForeignKey(nameof(Vehicle))]
        public Guid VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }


        [ForeignKey(nameof(Carrier))]
        public Guid CarrierId { get; set; }
        public virtual Carrier Carrier { get; set; }

        public string Phones { get; set; }
    }
}
