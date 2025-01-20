using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Company : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string InnKpp { get; set; }
        [MaxLength(500)]
        public string Address { get; set; }
        [MaxLength(200)]
        public string Phones { get; set; }
        [MaxLength(200)]
        public string Emails { get; set; }
    }

    [Table(nameof(Carrier))]
    public class Carrier : Company
    {
        public short Vat { get; set; }

        public virtual ICollection<Driver> Drivers { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }

        public virtual ICollection<Sub.File> Files { get; set; }
    }

    [Table(nameof(Client))]
    public class Client : Company
    {
        public bool IsPriority { get; set; }
    }
}
