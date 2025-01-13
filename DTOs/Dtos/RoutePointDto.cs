using DTOs.Dtos;

namespace DTOs
{
    public class RoutePointDto : IDto
    {
        public Guid Id { get; set; }
        public string Company { get; set; }
        public string Route { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Address { get; set; }
        public LoadingSide Side { get; set; }
        public LoadPointType Type { get; set; }
        public List<string> Phones { get; set; }
    }
}
