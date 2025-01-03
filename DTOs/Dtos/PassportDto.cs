namespace DTOs
{
    public class PassportDto : IDto
    {
        private string _validationError;
        public bool HasValidationError 
        {
            get 
            {
                _validationError = string.Empty;
                if (string.IsNullOrWhiteSpace(SerialNumber)) 
                {
                    _validationError = "Необходимо указать серию и номер паспорта";
                }
                else if (string.IsNullOrWhiteSpace(Issuer))
                {
                    _validationError = "Необходимо указать орган выдачи паспорта";
                }
                else if (DateOfIssue == DateTime.MinValue)
                {
                    _validationError = "Необходимо указать дату выдачи паспорта";
                }
                return string.IsNullOrWhiteSpace(_validationError);
            }
        }

        public string ValidationError => _validationError;


        public Guid Id { get; set; }
        public string SerialNumber { get; set; }
        public DateTime DateOfIssue { get; set; }
        public string Issuer { get; set; }
    }
}
