﻿namespace DTOs
{
    public class DriverDto : IDto
    {
        private string _validationError;

        public bool HasValidationError 
        {
            get 
            { 
                _validationError = string.Empty;
                string nameError = ModelsValidator.IsNameValid(Name);
                if (string.IsNullOrWhiteSpace(nameError))
                {
                    _validationError = nameError;
                }
                else if (BirthDate == DateTime.MinValue)
                {
                    _validationError = "Необходимо указать дату рождения";
                }
                else if (string.IsNullOrWhiteSpace(PassportSerial))
                {
                    _validationError = "Необходимо указать серию и номер паспорта";
                }
                else if (string.IsNullOrWhiteSpace(PassportIssuer))
                {
                    _validationError = "Необходимо указать орган выдачи паспорта";
                }
                else if (PassportDateOfIssue == DateTime.MinValue)
                {
                    _validationError = "Необходимо указать дату выдачи паспорта";
                }
                else if (Truck.HasValidationError)
                {
                    _validationError = Truck.ValidationError;
                }
                else if (Trailer.HasValidationError)
                {
                    _validationError = Trailer.ValidationError;
                }
                else
                {
                    if (!Phones.Any())
                    {
                        _validationError = "Необходимо указать телефон для связи";
                    }
                    else
                    {
                        foreach (var phone in Phones)
                        {
                            string phoneError = ModelsValidator.IsPhoneValid(phone);
                            if (string.IsNullOrWhiteSpace(phoneError))
                            {
                                _validationError = phoneError;
                                break;
                            }
                        }
                    }
                }
                return string.IsNullOrWhiteSpace(_validationError);
            }
        }

        public string ValidationError => _validationError;

        public Guid Id { get; set; }
        
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }

        public string PassportSerial { get; set; }
        public DateTime PassportDateOfIssue { get; set; }
        public string PassportIssuer { get; set; }

        public List<string> Phones { get; set; }


        public CarrierDto Carrier { get; set; }
        public TruckDto Truck { get; set; }
        public TrailerDto Trailer { get; set; }

       
    }
}
