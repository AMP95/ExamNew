using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class CompanyBase : BaseEntity
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
    public class Carrier : CompanyBase
    {
        public short Vat { get; set; }

        public virtual ICollection<Driver> Drivers { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
    }

    [Table(nameof(Company))]
    public class Company : CompanyBase
    {
        public short Type { get; set; }
    }

}
