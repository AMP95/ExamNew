
using Microsoft.AspNetCore.Http;

namespace DTOs.Dtos
{
    public class FileDto : IDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string SubFolder { get; set; }

        public Guid EntityId { get; set; }
        public Type EntityType { get; set; }

        public IFormFile File { get; set; }
    }
}
