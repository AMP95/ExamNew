using Models.Sub;

namespace Models.Main
{
    public class Template : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<AdditionalCondition> Additionals { get; set; }
    }
}
