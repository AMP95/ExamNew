namespace DTOs
{
    public class DocumentDto : IDto
    {
        private string _validationError;
        public bool HasValidationError 
        {
            get 
            { 
                _validationError = string.Empty;
                if (CreationDate == DateTime.MinValue)
                {
                    _validationError = "Необходимо указать дату создания документа";
                }
                else if (RecievingDate == DateTime.MinValue)
                {
                    _validationError = "Необходимо указать дату прихода документа";
                }
                else if (string.IsNullOrWhiteSpace(Number))
                {
                    _validationError = "Необходимо указать номер документа";
                }
                else if (Summ <= 0)
                {
                    _validationError = "Необходимо указать сумму документа";
                }
                else if (ContractId == Guid.Empty) 
                {
                    _validationError = "Необхождимо указать номер заявки";
                }
                return string.IsNullOrWhiteSpace(this._validationError);
            }
        }

        public string ValidationError => _validationError;

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
