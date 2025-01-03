namespace DTOs
{
    public class ContractDto : IDto
    {
        private string _validationError;
        public bool HasValidationError 
        {
            get 
            {
                _validationError = string.Empty;

                if (Number <= 0)
                {
                    _validationError = "Необходимо указать номер заявки";
                }
                else if (CreationDate == DateTime.MinValue)
                {
                    _validationError = "Необходимо указать дату создания заявки";
                }
                else if (LoadPoint == null)
                {
                    _validationError = "Необходимо указать точку погрузки";
                }
                else if (UnloadPoints == null && !UnloadPoints.Any())
                {
                    _validationError = "Необходимо указать точки выгрузки";
                }
                else if (Weight <= 0)
                {
                    _validationError = "Необходимо указать вес груза";
                }
                else if (Volume <= 0)
                {
                    _validationError = "Необходимо указать объем груза";
                }
                else if (Payment <= 0)
                {
                    _validationError = "Необходимо указать сумму оплаты";
                }
                else if (Carrier == null)
                {
                    _validationError = "Необходимо указать перевозчика";
                }
                else if (Driver == null)
                {
                    _validationError = "Необходимо указать водителя";
                }
                else if (Truck == null)
                {
                    _validationError = "Необходимо указать тягач";
                }
                else if (Trailer == null)
                {
                    _validationError = "Необходимо указать прицеп";
                }
                else
                {
                    foreach (RoutePointDto route in UnloadPoints) 
                    {
                        if (route.HasValidationError) 
                        { 
                            _validationError = route.ValidationError;
                            break;
                        }
                    }
                }

                return string.IsNullOrWhiteSpace(_validationError);
            }
        }

        public string ValidationError => _validationError;

        public Guid Id { get; set; }
        public short Number { get; set; }
        public DateTime CreationDate { get; set; }
        public ContractStatus Status { get; set; }

        public RoutePointDto LoadPoint { get; set; }
        public List<RoutePointDto> UnloadPoints { get; set; }

        public float Weight { get; set; }
        public float Volume { get; set; }

        public CarrierDto Carrier { get; set; }
        public DriverDto Driver { get; set; }
        public TruckDto Truck { get; set; }
        public TrailerDto Trailer { get; set; }

        public float Payment { get; set; }
        public float Prepayment { get; set; }
        public PaymentPriority PayPriority { get; set; }
        public RecievingType PaymentCondition { get; set; }
        public BookingDataDto BookingData { get; set; }

        
    }
}
