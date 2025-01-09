using DTOs.Dtos;

namespace DTOs
{
    public class DriverDto : IDto
    {
        public string this[string columnName] 
        {
            get 
            { 
                string error = string.Empty;

                switch (columnName) 
                {
                    case nameof(Name):
                        error = ModelsValidator.IsNameValid(Name);
                        break;
                    case nameof(BirthDate):
                        if (BirthDate == DateTime.MinValue)
                        {
                            error = "Необходимо указать дату рождения";
                        }
                        break;
                    case nameof(PassportSerial):
                        if (string.IsNullOrWhiteSpace(PassportSerial))
                        {
                            error = "Необходимо указать серию и номер паспорта";
                        }
                        break;
                    case nameof(PassportDateOfIssue):
                        if (PassportDateOfIssue == DateTime.MinValue)
                        {
                            error = "Необходимо указать дату выдачи паспорта";
                        }
                        break;
                    case nameof(PassportIssuer):
                        if (string.IsNullOrWhiteSpace(PassportIssuer))
                        {
                            error = "Необходимо указать орган выдачи паспорта";
                        }
                        break;
                    case nameof(Phones):
                        if (Phones != null && Phones.Any())
                        {
                            error = ModelsValidator.IsPhonesValid(Phones);
                        }
                        else 
                        {
                            error = "Необходимо указать телефон для связи";
                        }
                        break;
                }

                return error;
            }
        }

        public string Error => this[nameof(Name)] + 
                               this[nameof(BirthDate)] + 
                               this[nameof(PassportSerial)] + 
                               this[nameof(PassportDateOfIssue)] + 
                               this[nameof(PassportIssuer)] + 
                               this[nameof(Phones)];

        public Guid Id { get; set; }
        
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }

        public string PassportSerial { get; set; }
        public DateTime PassportDateOfIssue { get; set; }
        public string PassportIssuer { get; set; }

        public List<string> Phones { get; set; }


        public CarrierDto Carrier { get; set; }
        public VehicleDto Vehicle { get; set; }
    }
}
