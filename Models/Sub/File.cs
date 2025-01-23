namespace Models.Sub
{
    public class File : BaseEntity
    {
        public string ViewNameWithExtencion { get; set; }
        public string FullFilePath { get; set; }
        public Guid EntityId { get; set; }
        public string EntityType { get; set; }
    }
}
