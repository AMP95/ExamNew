using DTOs.Dtos;

namespace DTOs
{
    public class DocumentDto : IDto
    {
        public Guid Id { get; set; }
        public Guid ContractId { get; set; }
        public DocumentType Type { get; set; }
        public DocumentDirection Direction { get; set; }
        public DateTime CreationDate { get; set; }
        public RecievingType RecieveType { get; set; }
        public DateTime RecievingDate { get; set; }
        public string Number { get; set; }
        public float Summ { get; set; }

        
    }
}
