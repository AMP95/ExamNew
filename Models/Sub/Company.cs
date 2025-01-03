using System.ComponentModel.DataAnnotations;

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
        public string Phones { get;set; }
        [MaxLength(200)]
        public string Emails { get; set; }
    }
}
