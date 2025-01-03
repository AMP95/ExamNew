using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Document : BaseEntity
    {
        public short DocumentType { get; set; }
        public short DocumentDirection { get; set; }
        public DateTime CreationDate { get; set; }
        public short RecieveType { get; set; }
        public DateTime RecievingDate { get; set; }
        public string Number { get; set; }
        public float Summ { get; set; }


        [ForeignKey(nameof(Contract))]
        public Guid ContractId { get; set; }
        public Contract Contract { get; set; }
    }
}
