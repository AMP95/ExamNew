using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Driver : BaseEntity
    {

        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string FamilyName { get; set; }
        [MaxLength(50)]
        public string FatherName { get; set; }

        public DateTime DateOfBirth { get; set; }


        [MaxLength(50)]
        public string PassportSerial { get; set; }
        public DateTime PassportDateOfIssue { get; set; }
        [MaxLength(200)]
        public string PassportIssuer { get; set; }


        [ForeignKey(nameof(Vehicle))]
        public Guid? VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }


        [ForeignKey(nameof(Carrier))]
        public Guid? CarrierId { get; set; }
        public virtual Carrier Carrier { get; set; }


        [MaxLength(200)]
        public string Phones { get; set; }

        public virtual ICollection<Models.Sub.File> Files { get; set; }
    }
}
