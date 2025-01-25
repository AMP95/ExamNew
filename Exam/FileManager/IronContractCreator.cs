using DTOs;
using IronWord;
using MediatorServices;
using MediatorServices.Abstract;
using Models.Sub;
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

           

            Dictionary<string, string> replacements = new Dictionary<string, string>();

            replacements.Add(BookMarksList.GetValue("Номер"), contract.Number.ToString());
            replacements.Add(BookMarksList.GetValue("Дата"), contract.CreationDate.ToString("dd.MM.yyyy"));

            string route = contract.LoadPoint.Route;

            foreach (RoutePointDto point in contract.UnloadPoints) 
            {
                route += $" - {point.Route}"; 
            }

            replacements.Add(BookMarksList.GetValue("Маршрут"), route);


            replacements.Add(BookMarksList.GetValue("Вес"), contract.Weight.ToString());
            replacements.Add(BookMarksList.GetValue("Объем"), contract.Volume.ToString());

            string loadingPoint = $"Грузоотправитель: {contract.LoadPoint.Company}\n" +
                                    $"Адрес погрузки: {contract.LoadPoint.Address}\n" +
                                      $"Дата и время: {contract.LoadPoint.DateAndTime.ToString("dd.MM.yyyy HH:mm")}\n" +
                                          $"Контакты: {string.Join(";", contract.LoadPoint.Phones)}\n" +
                                   $"Способ погрузки: {contract.LoadPoint.Side.GetDescription()}";

            replacements.Add(BookMarksList.GetValue("Погрузка"), loadingPoint);

            string unloadingPoints =string.Empty;
            foreach (RoutePointDto point in contract.UnloadPoints) 
            {
                unloadingPoints += $"Грузополучатель: {point.Company}\n" +
                                    $"Адрес выгрузки: {point.Address}\n" +
                                      $"Дата и время: {point.DateAndTime.ToString("dd.MM.yyyy HH:mm")}\n" +
                                          $"Контакты: {string.Join(";", point.Phones)}\n" +
                                   $"Способ выгрузки: {point.Side.GetDescription()}\n\n";
            }

            replacements.Add(BookMarksList.GetValue("Выгрузка"), unloadingPoints);

            string payment = $"{contract.Payment} руб. {contract.Carrier.Vat.GetDescription()}";

            if (contract.Prepayment > 0) 
            {
                payment += $"\nПредоплата: {contract.Prepayment.ToString()} руб.";
            }

            replacements.Add(BookMarksList.GetValue("Оплата"), payment);
            replacements.Add(BookMarksList.GetValue("Условия оплаты"), $"{contract.PaymentCondition.GetDescription()}, {contract.PayPriority.GetDescription()}" );

            string driver = $"ФИО: {contract.Driver.Name}\n" +
                            $"Паспорт: {contract.Driver.PassportSerial}," +
                            $"выдан {contract.Driver.PassportIssuer}, {contract.Driver.PassportDateOfIssue.ToString("dd.MM.yyyy")}\n" +
                            $"Тел.: {string.Join(";", contract.Driver.Phones)}";

            replacements.Add(BookMarksList.GetValue("Водитель"), driver);

            replacements.Add(BookMarksList.GetValue("ТС"), $"{contract.Vehicle.TruckModel} {contract.Vehicle.TruckNumber}, {contract.Vehicle.TrailerNumber}");

            string carrier = $"Перевозчик: {contract.Carrier.Name}\n" +
                             $"Адрес: {contract.Carrier.Address}\n" +
                             $"Инн/КПП: {contract.Carrier.InnKpp}\n" +
                             $"Тел.: {string.Join(";", contract.Carrier.Phones)}\n" +
                             $"E-mail: {string.Join(";", contract.Carrier.Emails)}";

            replacements.Add(BookMarksList.GetValue("Перевозчик"), carrier);

            WordDocument doc = new WordDocument(fullPath);

            foreach (var replacement in replacements)
            {
                doc.Texts.ForEach(x => x.Replace(replacement.Key, replacement.Value));
            }

            string cntractPath = Path.Combine(nameof(Contract), contract.CreationDate.Year.ToString(), $"{contract.Number}.docx");
            string fullSavePath = _fileManager.GetFullPath(cntractPath);

            Directory.CreateDirectory(Path.GetDirectoryName(fullSavePath));

            doc.Save(fullSavePath);

            return cntractPath;
        }
    }
}
