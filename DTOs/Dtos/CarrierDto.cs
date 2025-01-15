using DTOs.Dtos;

namespace DTOs
{
    public class CompanyDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string InnKpp { get; set; }
        public string Address { get; set; }
        public List<string> Phones { get; set; }
        public List<string> Emails { get; set; }
    }

    public class CarrierDto : CompanyDto
    {
        public VAT Vat { get; set; }
        public List<VehicleDto> Vehicles { get; set; }
    }

    public class ClientDto : CompanyDto { }
}
