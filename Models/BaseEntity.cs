using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
