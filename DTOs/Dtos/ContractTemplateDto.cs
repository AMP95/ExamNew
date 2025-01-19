namespace DTOs.Dtos
{
    public class ContractTemplateDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public FileDto File { get; set; }
    }
}
