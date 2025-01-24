using DTOs;
using DTOs.Dtos;
using Exam.Interfaces;
using IronWord;
using MediatorServices.Abstract;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Exam.FileManager
{
    #region Extend

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum @enum)
        {
            FieldInfo fi = @enum.GetType().GetField(@enum.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return @enum.ToString();
        }

        public static string GetDescription<TEnum>(this object @object) where TEnum : struct
        {
            return Enum.TryParse(@object.ToString(), out TEnum @enum) ? (@enum as Enum).GetDescription() : @object.ToString();
        }
    }

    public class EnumDescriptionTypeConverter : EnumConverter
    {
        public EnumDescriptionTypeConverter(Type type)
            : base(type)
        {
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value != null)
                {
                    return (value as Enum).GetDescription();
                }

                return string.Empty;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    #endregion Extend

    public class IronContractCreator : IContractCreator
    {
        private IFileManager _fileManager;

        public IronContractCreator(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }
        public string CreateContractDocument(ContractDto contract, string templatePathWithoutRoot)
        {
            string fullPath = _fileManager.GetFullPath(templatePathWithoutRoot);

            WordDocument doc = new WordDocument(fullPath);

            Dictionary<string, string> replacements = new Dictionary<string, string>();

            replacements.Add("{{ContractNumber}}", contract.Number.ToString());
            replacements.Add("{{ConractDate}}", contract.CreationDate.ToString("dd.MM.yyyy"));
            replacements.Add("{{CarrierTop}}", contract.Carrier.Name);

            //TO DO
            replacements.Add("{{Route}}", contract.Number.ToString());


            replacements.Add("{{Weight}}", contract.Weight.ToString());
            replacements.Add("{{Volume}}", contract.Volume.ToString());

            replacements.Add("{{CargoSender}}", contract.LoadPoint.Company.ToString());
            replacements.Add("{{LoadingPoint}}", contract.LoadPoint.Address.ToString());
            replacements.Add("{{LoadingDate}}", contract.LoadPoint.DateAndTime.ToString("dd.MM.yyyy HH:mm"));
            replacements.Add("{{SenderPhone}}", string.Join(";", contract.LoadPoint.Phones));
            replacements.Add("{{LoadingType}}", contract.LoadPoint.Side.GetDescription());

            //TO DO
            replacements.Add("{{CargoReciever}}", contract.Number.ToString());
            replacements.Add("{{UnloadingPoint}}", contract.Number.ToString());
            replacements.Add("{{UnloadingDate}}", contract.Number.ToString());
            replacements.Add("{{RecievePhone}}", contract.Number.ToString());
            replacements.Add("{{UnloadType}}", contract.Number.ToString());


            replacements.Add("{{Payment}}", $"{contract.Payment} руб., предоплата {contract.Prepayment} руб.");
            replacements.Add("{{PaymentCondition}}", contract.PaymentCondition.GetDescription());
            replacements.Add("{{PayPriority}}", contract.PayPriority.GetDescription());

            replacements.Add("{{Driver}}", contract.Driver.Name);
            replacements.Add("{{DriverPassport}}", contract.Driver.PassportSerial + contract.Driver.PassportIssuer + contract.Driver.PassportDateOfIssue.ToString("dd.MM.yyyy"));
            replacements.Add("{{DriverPone}}", string.Join(";", contract.Driver.Phones));

            replacements.Add("{{Vehicle}}", $"{contract.Vehicle.TruckModel} {contract.Vehicle.TruckNumber}, {contract.Vehicle.TrailerNumber}");

            replacements.Add("{{CarrierBottom}}", contract.Carrier.Name);
            replacements.Add("{{CarrierAddress}}", contract.Carrier.Address);
            replacements.Add("{{CarrierInnKpp}}", contract.Carrier.InnKpp);
            replacements.Add("{{CarrierPhone}}", string.Join(";", contract.Carrier.Phones));
            replacements.Add("{{CarrierMail}}", string.Join(";", contract.Carrier.Emails));

            foreach (var replacement in replacements)
            {
                doc.Texts.ForEach(x => x.Replace(replacement.Key, replacement.Value));
            }

            string cntractPath = Path.Combine(nameof(Contract), contract.CreationDate.Year.ToString(), $"{contract.Number}.docx");
            string fullSavePath = _fileManager.GetFullPath(cntractPath);

            doc.Save(fullSavePath);

            return fullSavePath;
        }
    }
}
