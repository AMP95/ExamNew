using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class DocumentBase : BaseEntity 
    {
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public short DocumentDirection { get; set; }

        public float Summ { get; set; }


        [ForeignKey(nameof(Contract))]
        public Guid ContractId { get; set; }
        public Contract Contract { get; set; }
    }

    [Table(nameof(Document))]
    public class Document : DocumentBase
    {
        public short DocumentType { get; set; }
        public short RecieveType { get; set; }
        public DateTime RecievingDate { get; set; }
    }

    [Table(nameof(Payment))]
    public class Payment : DocumentBase 
    { 
        
    }
}
