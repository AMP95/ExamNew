namespace DTOs.Dtos
{
    public class AdditionalDto : IDto
    {
        public Guid Id { get; set ; }
        public string Name { get; set; }
        public string Description { get; set; }

    }

    public class TemplateDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<AdditionalDto> Additionals { get; set; }
        public FileDto File { get; set; }
    }
}
