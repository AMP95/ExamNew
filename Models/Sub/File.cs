namespace Models.Sub
{
    public class File : BaseEntity
    {
        public string Name { get; set; }
        public string Extencion { get; set; }
        public string Subfolder { get; set; }
        public Guid EntityId { get; set; }
        public string EntityType { get; set; }
    }
}
