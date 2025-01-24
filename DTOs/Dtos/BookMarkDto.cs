namespace DTOs.Dtos
{
    public class BookMarkDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string InsertView { get; set; }
    }
}
