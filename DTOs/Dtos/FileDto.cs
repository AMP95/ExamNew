
namespace DTOs.Dtos
{
    public class FileDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SaveName { get; set; }
    }
}
