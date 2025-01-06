using DTOs.Dtos;

namespace DTOs
{
    public class CompanyDto : IDto
    {
        public string this[string columnName] 
        {
            get 
            { 
                string error = string.Empty;

                switch (columnName) 
                {
                    case nameof(Name):
                        if (string.IsNullOrWhiteSpace(Name))
                        {
                            error = "Необходимо указать название контрагента";
                        }
                        break;
                    case nameof(InnKpp):
                        error = ModelsValidator.IsInnKppValid(InnKpp);
                        break;
                    case nameof(Address):
                        if (string.IsNullOrWhiteSpace(Address))
                        {
                            error = "Необходимо указать адрес контрагента";
                        }
                        break;
                    case nameof(Phones):
                        if (Phones.Any())
                        {
                            error = ModelsValidator.IsPhonesValid(Phones);
                        }
                        else 
                        {
                            error = "Необходимо указать телефон для связи";
                        }
                        break;
                    case nameof(Emails):
                        if (Emails.Any())
                        {
                            error = ModelsValidator.IsMailsValid(Emails);
                        }
                        else 
                        {
                            error = "Необходимо указать email";
                        }
                        break;

                }

                return error;
            }
        }

        public string Error => this[nameof(Name)] + 
                               this[nameof(InnKpp)] + 
                               this[nameof(Address)] + 
                               this[nameof(Phones)] + 
                               this[nameof(Emails)];

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string InnKpp { get; set; }
        public string Address { get; set; }
        public List<string> Phones { get; set; }
        public List<string> Emails { get; set; }

       
    }
}
