using Models.Sub;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{


    public class Contract
    {
        [Key]
        public Guid Id { get; set; }
        public short Number { get; set; }
        public DateTime CreationDate { get; set; }
        public ContractStatus Status { get; set; }


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



        [ForeignKey(nameof(Truck))]
        public Guid TruckId { get; set; }
        public Truck Truck { get; set; }



        [ForeignKey(nameof(Trailer))]
        public Guid TrailerId { get; set; }
        public Trailer Trailer { get; set; }


        public float Payment { get; set; }
        public float Prepayment { get; set; }
        public PaymentPriority PayPriority { get; set; }
        public RecievingType PaymentCondition { get; set; }


        [ForeignKey(nameof(BookingData))]
        public Guid? BookingDataId { get; set; }
        public virtual BookingData BookingData { get; set; }
    }
}
