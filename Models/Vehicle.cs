using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class VehiclePart
    {
        [Key]
        public Guid Id { get; set; }
        public string Model { get; set; }
        public string Number { get; set; }

        [ForeignKey(nameof(Carrier))]
        public Guid? CarrierId { get; set; }
        public virtual Carrier Carrier { get; set; }
    }

    [Table(nameof(Truck))]
    public class Truck : VehiclePart { }

    [Table(nameof(Trailer))]
    public class Trailer : VehiclePart { }
}
