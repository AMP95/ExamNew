using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Document 
    {
        [Key]
        public Guid Id { get; set; }
        public short DocumentType { get; }
        public DateTime CreationDate { get; set; }
        public short RecieveType { get; }
        public DateTime RecievingDate { get; set; }
        public string Number { get; set; }
        public float Summ { get; set; }
    }
}
