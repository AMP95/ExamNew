using DTOs.Dtos;
using System.ComponentModel;

namespace DTOs
{
    public class DocumentDto : IDto
    {
        public string this[string columnName] 
        {
            get 
            { 
                string error = string.Empty;

                switch (columnName) 
                {
                    case nameof(ContractId):
                        if (ContractId == Guid.Empty) 
                        {
                            error = "Необходумо указать номер заявки\n";
                        }
                        break;
                    case nameof(CreationDate):
                        if (CreationDate == DateTime.MinValue)
                        {
                            error = "Необходимо указать дату создания документа\n";
                        }
                        break;
                    case nameof(Number):
                        if(string.IsNullOrWhiteSpace(Number))
                        {
                            error = "Необходимо указать номер документа\n";
                        }
                        break;
                    case nameof(RecievingDate):
                        if (RecievingDate == DateTime.MinValue)
                        {
                            error = "Необходимо указать дату прихода документа\n";
                        }
                        break;
                    case nameof(Summ):
                        if (Summ <= 0)
                        {
                            error = "Необходимо указать сумму документа\n";
                        }
                        break;
                }

                return error;
            }
        }

        public string Error => this[nameof(ContractId)] +
                               this[nameof(CreationDate)] + 
                               this[nameof(RecievingDate)] + 
                               this[nameof(Number)] + 
                               this[nameof(Summ)];

        public Guid Id { get; set; }
        public Guid ContractId { get; set; }
        public DocumentType Type { get; set; }
        public DocumentDirection Direction { get; set; }
        public DateTime CreationDate { get; set; }
        public RecievingType RecieveType { get; set; }
        public DateTime RecievingDate { get; set; }
        public string Number { get; set; }
        public float Summ { get; set; }

        
    }
}
