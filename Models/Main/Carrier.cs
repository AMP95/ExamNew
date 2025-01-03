using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{

    [Table(nameof(Carrier))]
    public class Carrier : Company
    {
        public short Vat { get; set; }

        public virtual ICollection<Driver> Drivers { get; set; }
        public virtual ICollection<Truck> Trucks { get; set; }
        public virtual ICollection<Trailer> Trailers { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
    }
}
