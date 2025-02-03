using DTOs.Dtos;


namespace DTOs
{
    public class RequiredToPayContractDto : IDto
    {
        public Guid Id { get; set; }
        public short Number { get; set; }
        public DateTime CreationDate { get; set; }
        public string Carrier { get; set; }
        public string Driver { get; set; }
        public string Vehicle { get; set; }
        public double Balance { get; set; }
        public PayType Type { get; set; }
        public int DaysToExpiration { get; set; }
    }

    

    public class ContractDto : IDto
    {
        public Guid Id { get; set; }
        public short Number { get; set; }
        public DateTime CreationDate { get; set; }
        public ContractStatus Status { get; set; }

        public RoutePointDto LoadPoint { get; set; }
        public List<RoutePointDto> UnloadPoints { get; set; }

        public float Weight { get; set; }
        public float Volume { get; set; }

        public CarrierDto Carrier { get; set; }
        public CompanyDto Client { get; set; }
        public DriverDto Driver { get; set; }
        public VehicleDto Vehicle { get; set; }
        public LogistDto Logist { get; set; }
        public TemplateDto Template { get; set; }

        public float Payment { get; set; }
        public float ClientPayment { get; set; }
        public float Prepayment { get; set; }
        public PaymentPriority PayPriority { get; set; }
        public RecievingType PaymentCondition { get; set; }
        public List<DocumentDto> Documents { get; set; }
    }
}
