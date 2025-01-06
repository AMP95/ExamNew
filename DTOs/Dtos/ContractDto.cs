using DTOs.Dtos;
using System.ComponentModel;

namespace DTOs
{
    public class ContractDto : IDto
    {
        public string this[string columnName]
        {
            get 
            { 
                string error = string.Empty;

                switch (columnName) 
                {
                    case nameof(Number):
                        if (Number <= 0)
                        {
                            error = "Необходимо указать номер заявки";
                        }
                        break;
                    case nameof(CreationDate):
                        if (CreationDate == DateTime.MinValue)
                        {
                            error = "Необходимо указать дату создания заявки";
                        }
                        break;
                    case nameof(LoadPoint):
                        if (LoadPoint == null)
                        {
                            error = "Необходимо указать точку погрузки";
                        }
                        else 
                        { 
                            error = LoadPoint.Error;
                        }
                        break;
                    case nameof(UnloadPoints):
                        if (UnloadPoints.Any())
                        {
                            RoutePointDto point = UnloadPoints.FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.Error));
                            if (point != null) 
                            { 
                                error = point.Error;
                            }
                        }
                        else 
                        {
                            error = "Необходимо указать точку выгрузки";
                        }
                        break;
                    case nameof(Weight):
                        if (Weight <= 0)
                        {
                            error = "Необходимо указать вес груза";
                        }
                        break;
                    case nameof(Volume):
                        if (Volume <= 0)
                        {
                            error = "Необходимо указать объем груза";
                        }
                        break;
                    case nameof(Carrier):
                        if (Carrier == null)
                        {
                            error = "Необходимо указать перевозчика";
                        }
                        break;
                    case nameof(Driver):
                        if (Driver == null)
                        {
                            error = "Необходимо указать водителя";
                        }
                        break;
                    case nameof(Vehicle):
                        if (Vehicle == null)
                        {
                            error = "Необходимо указать ТС";
                        }
                        break;
                    case nameof(Payment):
                        if (Payment <= 0)
                        {
                            error = "Необходимо указать сумму оплаты";
                        }
                        break;
                }

                return error;
            }
        }

        public string Error => this[nameof(Number)] +
                               this[nameof(CreationDate)] + 
                               this[nameof(LoadPoint)] + 
                               this[nameof(UnloadPoints)] + 
                               this[nameof(Weight)] + 
                               this[nameof(Volume)] + 
                               this[nameof(Carrier)] + 
                               this[nameof(Driver)] + 
                               this[nameof(Vehicle)] + 
                               this[nameof(Payment)];

        public Guid Id { get; set; }
        public short Number { get; set; }
        public DateTime CreationDate { get; set; }
        public ContractStatus Status { get; set; }

        public RoutePointDto LoadPoint { get; set; }
        public List<RoutePointDto> UnloadPoints { get; set; }

        public float Weight { get; set; }
        public float Volume { get; set; }

        public CarrierDto Carrier { get; set; }
        public DriverDto Driver { get; set; }
        public VehicleDto Vehicle { get; set; }

        public float Payment { get; set; }
        public float Prepayment { get; set; }
        public PaymentPriority PayPriority { get; set; }
        public RecievingType PaymentCondition { get; set; }
        public List<DocumentDto> Documents { get; set; }

        
    }
}
