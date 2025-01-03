namespace Models.Sub
{
    public class RoutePoint : BaseEntity
    {
        public string Route { get; set; }
        public string Address { get; set; }
        public short Side { get; set; }
        public short Type { get; set; }
        public string Phones { get; set; }
    }
}
