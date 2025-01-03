using System.ComponentModel.DataAnnotations;

namespace Models.Sub
{
    public class Passport
    {
        [Key]
        public Guid Id { get; set; }
        public string SerialNumber { get; set; }
        public DateTime DateOfIssue { get; set; }
        public string Issuer { get; set; }
    }
}
