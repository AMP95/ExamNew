using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Main
{
    public class ContractTemplate : BaseEntity
    {
        public string Name { get; set; }


        [ForeignKey(nameof(File))]
        public Guid? FileId { get; set; }
        public Models.Sub.File File { get; set; }
    }
}
