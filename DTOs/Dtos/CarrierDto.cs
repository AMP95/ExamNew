using DTOs.Dtos;

namespace DTOs
{
    public class CompanyBaseDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string InnKpp { get; set; }
        public string Address { get; set; }
        public List<string> Phones { get; set; }
        public List<string> Emails { get; set; }
    }

    public class CarrierDto : CompanyBaseDto
    {
        public VAT Vat { get; set; }
        public List<VehicleDto> Vehicles { get; set; }
    }

    public class CompanyDto : CompanyBaseDto 
    {
        public CompanyType Type { get; set; }
    }

}
