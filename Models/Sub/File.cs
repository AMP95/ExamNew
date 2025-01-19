namespace Models.Sub
{
    public class File : BaseEntity
    {
        public string Path { get; set; }
        public Type EntityType { get; set; }
        public Guid EntityId { get; set; }
    }
}
