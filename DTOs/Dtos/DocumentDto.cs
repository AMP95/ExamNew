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
                return string.IsNullOrWhiteSpace(this._validationError);
            }
        }

        public string ValidationError => _validationError;

        public Guid Id { get; set; }
        public DocumentType DocumentType { get; }
        public DateTime CreationDate { get; set; }
        public RecievingType RecieveType { get; }
        public DateTime RecievingDate { get; set; }
        public string Number { get; set; }
        public float Summ { get; set; }
    }
}
