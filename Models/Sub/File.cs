using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Sub
{
    public class File : BaseEntity
    {
        public string Name { get; set; }
        public string SaveName { get; set; }


        [ForeignKey(nameof(Entity))]
        public Guid? EntityId { get; set; }
        public virtual BaseEntity Entity { get; set; }
    }
}
