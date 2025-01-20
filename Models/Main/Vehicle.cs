using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Vehicle : BaseEntity
    {
        [MaxLength(50)]
        public string TruckModel { get; set; }
        [MaxLength(20)]
        public string TruckNumber { get; set; }

        [MaxLength(50)]
        public string TrailerModel { get; set; }
        [MaxLength(20)]
        public string TrailerNumber { get; set; }


        [ForeignKey(nameof(Carrier))]
        public Guid? CarrierId { get; set; }
        public virtual Carrier Carrier { get; set; }


        public virtual ICollection<Sub.File> Files { get; set; }
    }
}
