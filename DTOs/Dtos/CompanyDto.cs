namespace DTOs
{
    public class CompanyDto : IDto
    {
        private string _validationError;
        public bool HasValidationError 
        { 
            get
            { 
                _validationError = string.Empty;
                if (string.IsNullOrWhiteSpace(Name))
                {
                    _validationError = "Необходимо указать название контрагента";
                }
                else if (string.IsNullOrWhiteSpace(Address))
                {
                    _validationError = "Необходимо указать адрес контрагента";
                }
                else 
                {
                    string innKppError = ModelsValidator.IsInnKppValid(InnKpp);
                    string phoneError = ModelsValidator.IsPhonesValid(Phones);
                    string mailError = ModelsValidator.IsMailsValid(Emails);

                    if (string.IsNullOrWhiteSpace(innKppError))
                    {
                        _validationError = innKppError;
                    }
                    else if (string.IsNullOrWhiteSpace(phoneError))
                    {
                        _validationError = phoneError;
                    }
                    else if (string.IsNullOrWhiteSpace(mailError)) 
                    {
                        _validationError = mailError;
                    }
                }
                return string.IsNullOrWhiteSpace(_validationError);
            }
        }

        public string ValidationError => _validationError;

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string InnKpp { get; set; }
        public string Address { get; set; }
        public List<string> Phones { get; set; }
        public List<string> Emails { get; set; }
    }
}
