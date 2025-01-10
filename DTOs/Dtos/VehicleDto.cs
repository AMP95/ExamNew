using DTOs.Dtos;

namespace DTOs
{
    public class VehicleDto : IDto
    {
        public Guid Id { get; set; }
        public CarrierDto Carrier { get; set; }
        public string TruckModel { get; set; }
        public string TruckNumber { get; set; }
        public string TrailerModel { get; set; }
        public string TrailerNumber { get; set; }

    }
}
