namespace DTOs
{
    public class TrailerDto : IDto
    {
        private string _validationError;

        public Guid Id { get; set; }
        public CarrierDto Carrier { get; set; }
        public string Model { get; set; }
        public string Number { get; set; }
        public bool HasValidationError
        {
            get
            {
                _validationError = ModelsValidator.IsTrailerNumberValid(Number);
                return string.IsNullOrWhiteSpace(_validationError);
            }
        }
        public string ValidationError { get => _validationError; }

    }
}
