using DTOs.Dtos;

namespace DTOs
{
    public class DocumentBaseDto : IDto 
    {
        public Guid Id { get; set; }
        public Guid ContractId { get; set; }
        public string Number { get; set; }

        public DateTime CreationDate { get; set; }
        public DocumentDirection Direction { get; set; }
        public float Summ { get; set; }
    }

    public class DocumentDto : DocumentBaseDto
    {
        public DocumentType Type { get; set; }
        public RecievingType RecieveType { get; set; }
        public DateTime RecievingDate { get; set; }
    }

    public class PaymentDto : DocumentBaseDto { }
}
