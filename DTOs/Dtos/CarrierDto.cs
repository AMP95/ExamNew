namespace DTOs
{
    public class CarrierDto : CompanyDto
    {
        public VAT Vat { get; set; }
        public List<VehicleDto> Vehicles { get; set; }
    }
}
