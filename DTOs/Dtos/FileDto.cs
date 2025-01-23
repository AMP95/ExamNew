
using Microsoft.AspNetCore.Http;

namespace DTOs.Dtos
{
    public class FileDto : IDto
    {
        public Guid Id { get; set; }
        public string FileNameWithExtencion { get; set; }
        public string Catalog { get; set; }

        public Guid DtoId { get; set; }
        public string DtoType { get; set; }

        public IFormFile File { get; set; }
    }
}
