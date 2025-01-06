namespace DTOs
{
    public class CarrierDto : CompanyDto
    {
        public VAT Vat { get; set; }
        public List<TruckDto> Trucks { get; set; }
        public List<TrailerDto> Trailers { get; set; }
    }
}
