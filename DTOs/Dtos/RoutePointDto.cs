namespace DTOs
{
    public class RoutePointDto : IDto
    {
        private string _validationError;
        public bool HasValidationError 
        {
            get 
            { 
                _validationError = string.Empty;
                if (string.IsNullOrWhiteSpace(Address))
                {
                    _validationError = "Необходимо указать адрес";
                }
                else 
                {
                    _validationError = ModelsValidator.IsPhonesValid(Phones);
                }
                return string.IsNullOrWhiteSpace(_validationError);
            }
        }

        public string ValidationError => _validationError;

        public Guid Id { get; set; }
        public string Address { get; set; }
        public LoadingSide Side { get; set; }
        public LoadPointType Type { get; set; }
        public List<string> Phones { get; set; }
    }
}
