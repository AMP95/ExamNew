namespace DTOs
{
    public class BookingDataDto : IDto
    {
        public Guid Id { get; set; }

        public List<DocumentDto> IncomeDocuments { get; set; }
        public DocumentDto OutcomeDocument { get; set; }

        public List<DocumentDto> OutcomePayments { get; set; }
        public DocumentDto IncomePayment { get; set; }


        public bool HasValidationError => false;

        public string ValidationError => string.Empty;
    }
}
