using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class BookingData
    {
        [Key]
        public Guid Id { get; set; }

        public virtual ICollection<Document> IncomeDocuments { get; set; }
        

        [ForeignKey(nameof(OutcomeDocument))]
        public Guid? OutcomeDocumentId { get; set; }
        public virtual Document OutcomeDocument { get; set; }


        public virtual ICollection<Document> OutcomePayments { get; set; }


        [ForeignKey(nameof(IncomePayment))]
        public Guid? IncomePaymentId { get; set; }
        public virtual Document IncomePayment { get; set; }

    }
}
