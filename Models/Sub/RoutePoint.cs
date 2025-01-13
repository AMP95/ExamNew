using System.ComponentModel.DataAnnotations;

namespace Models.Sub
{
    public class RoutePoint : BaseEntity
    {
        [MaxLength(50)]
        public string Company { get; set; }

        [MaxLength(50)]
        public string Route { get; set; }
        [MaxLength(200)]
        public string Address { get; set; }
        public short Side { get; set; }
        public short Type { get; set; }
        [MaxLength(200)]
        public string Phones { get; set; }
    }
}
