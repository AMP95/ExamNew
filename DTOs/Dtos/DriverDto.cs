using DTOs.Dtos;

namespace DTOs
{
    public class DriverDto : IDto
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }

        public string PassportSerial { get; set; }
        public DateTime PassportDateOfIssue { get; set; }
        public string PassportIssuer { get; set; }

        public List<string> Phones { get; set; }


        public CarrierDto Carrier { get; set; }
        public VehicleDto Vehicle { get; set; }
    }
}
