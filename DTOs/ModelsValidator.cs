using System.Text.RegularExpressions;

namespace DTOs
{
    internal static class ModelsValidator
    {
        private static Regex _phone;
        private static Regex _mail;
        private static Regex _truck;
        private static Regex _trailer;
        private static Regex _name;
        private static Regex _innKpp;
        static ModelsValidator() 
        {
            _phone = new Regex("^\\+([1-9]{1})([0-9]*)((-[0-9]{3}){2})((-[0-9]{2}){2})$");
            _mail = new Regex("^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$");
            _truck = new Regex("^[A-Z][ ]([0-9]{3})[ ]([A-Z]{2})\\/([0-9]{2,3})$");
            _trailer = new Regex("^[A-Z]{2}[ ]([0-9]{4})\\/([0-9]{2})$");
            _name = new Regex("^([\\w]*([-][\\w]*)?)([ ]([\\w]*([-][\\w]*)?))+$");
            _innKpp = new Regex("^[0-9]+(\\/[0-9]+)*$");
        }
        public static string IsTruckNumberValid(string truckNumber) 
        {
            if (_truck.IsMatch(truckNumber))
            {
                return string.Empty;
            }
            else 
            {
                return "Номер должен соответствовать шаблону: A 000 AA/54(154)\n";
            }
        }

        public static string IsTrailerNumberValid(string trailerNumber)
        {
            if (_trailer.IsMatch(trailerNumber))
            {
                return string.Empty;
            }
            else
            {
                return "Номер должен соответствовать шаблону: AA 0000/54\n";
            }
        }

        public static string IsNameValid(string name)
        {
            if (_name.IsMatch(name))
            {
                return string.Empty;
            }
            else
            {
                return "Имя должно соответствовать шаблону: Фамилия Имя (Отчество)\n";
            }
        }

        public static string IsPhonesValid(List<string> phones)
        {
            string errored = phones.FirstOrDefault(m => !string.IsNullOrWhiteSpace(IsPhoneValid(m)));

            if (errored != default)
            {
                return $"Номер телефона {errored} должен соответствовать шаблону: +7-000-000-00-00\n";
            }
            else
            {
                return string.Empty;
            }
        }

        public static string IsPhoneValid(string phone)
        {
            if (_phone.IsMatch(phone))
            {
                return string.Empty;
            }
            else
            {
                return "Номер телефона должен соответствовать шаблону: +7-000-000-00-00\n";
            }
        }

        public static string IsMailValid(string email)
        {
            if (_mail.IsMatch(email))
            {
                return string.Empty;
            }
            else
            {
                return "Неверный формат email\n";
            }
        }

        public static string IsMailsValid(List<string> emails)
        {
            string errored = emails.FirstOrDefault(m => !string.IsNullOrWhiteSpace(IsMailValid(m)));

            if (errored != default)
            {
                return $"Неверный формат email: {errored}\n";
            }
            else 
            {
                return string.Empty;
            }
        }
        public static string IsInnKppValid(string innKpp)
        {
            if (_innKpp.IsMatch(innKpp))
            {
                return string.Empty;
            }
            else
            {
                return "Номер инн/кпп соответствовать шаблону: ИНН(/КПП)\n";
            }
        }
    }
}
