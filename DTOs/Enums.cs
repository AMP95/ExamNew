using System.ComponentModel;

namespace DTOs
{
    public enum DocumentType
    {
        [Description("Счет")]
        Bill,
        [Description("Акт")]
        Act,
        [Description("Счет-фактура")]
        Invoice,
        [Description("УПД")]
        UDT,
        [Description("ТТН")]
        TTN
    }

    public enum PaymentPriority
    {
        [Description("5-7 б/д")]
        Normal,
        [Description("3-5 б/д")]
        Hight,
        [Description("1-2 б/д")]
        ExtraHight
    }

    public enum RecievingType
    {
        [Description("Оригиналы")]
        Originals,
        [Description("Сканы")]
        Scans
    }

    public enum ContractStatus
    {
        Created,
        DocumentsRecieved,
        DocumentSended,
        Closed
    }

    public enum LoadingSide
    {
        [Description("Зад")]
        Back,
        [Description("Бок")]
        Side,
        [Description("Верх")]
        Top
    }

    public enum LoadPointType 
    {
        [Description("Загрузка")]
        Upload,
        [Description("Выгрузка")]
        Download
    }

    public enum VAT
    {
        [Description("Без НДС")]
        Without,
        [Description("С НДС")]
        With
    }
}
