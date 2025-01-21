using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Sub
{
    public class File : BaseEntity
    {
        public string Name { get; set; }
        public string SaveName { get; set; }
    }
}
