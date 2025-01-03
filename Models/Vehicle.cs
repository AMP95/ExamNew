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
    }

    [Table(nameof(Truck))]
    public class Truck : VehiclePart { }

    [Table(nameof(Trailer))]
    public class Trailer : VehiclePart { }


    public class Vehicle
    {
        [Key]
        public Guid Id { get; set; }


        [ForeignKey(nameof(Truck))]
        public Guid TruckId { get; set; }
        public virtual Truck Truck { get; set; }


        [ForeignKey(nameof(Trailer))]
        public Guid TrailerId { get; set; }
        public virtual Trailer Trailer { get; set; }
    }
}
