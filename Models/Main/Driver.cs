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


        [ForeignKey(nameof(Truck))]
        public Guid? TruckId { get; set; }
        public Truck Truck { get; set; }



        [ForeignKey(nameof(Trailer))]
        public Guid? TrailerId { get; set; }
        public Trailer Trailer { get; set; }



        [ForeignKey(nameof(Carrier))]
        public Guid? CarrierId { get; set; }
        public virtual Carrier Carrier { get; set; }

        [MaxLength(200)]
        public string Phones { get; set; }
    }
}
