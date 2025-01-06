using Models.Sub;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Contract : BaseEntity
    {
        public short Number { get; set; }
        public DateTime CreationDate { get; set; }
        public short Status { get; set; }


        [ForeignKey(nameof(LoadingPoint))]
        public Guid LoadingPointId { get; set; }
        public RoutePoint LoadingPoint { get; set; }


        public virtual ICollection <RoutePoint> UnloadingPoints { get; set; }

        public float Weight { get; set; }
        public float Volume { get; set; }


        [ForeignKey(nameof(Carrier))]
        public Guid CarrierId { get; set; }
        public Carrier Carrier { get; set; }



        [ForeignKey(nameof(Driver))]
        public Guid DriverId { get; set; }
        public Driver Driver { get; set; }



        [ForeignKey(nameof(Vehicle))]
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }


        public float Payment { get; set; }
        public float Prepayment { get; set; }
        public short PayPriority { get; set; }
        public short PaymentCondition { get; set; }


        public virtual ICollection<Document> Documents { get; set; }
    }
}
