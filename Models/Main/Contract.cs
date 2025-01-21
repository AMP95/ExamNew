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


        [ForeignKey(nameof(Client))]
        public Guid ClientId { get; set; }
        public Client Client { get; set; }


        [ForeignKey(nameof(Driver))]
        public Guid DriverId { get; set; }
        public Driver Driver { get; set; }


        [ForeignKey(nameof(Vehicle))]
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public float CarrierPayment { get; set; }
        public float CarrierPrepayment { get; set; }
        public short CarrierPayPriority { get; set; }
        public short CarrierPaymentCondition { get; set; }
        public float ClientPayment { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
