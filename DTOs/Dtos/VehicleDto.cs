using DTOs.Dtos;

namespace DTOs
{
    public class VehicleDto : IDto
    {
        public virtual string this[string columnName] 
        {
            get 
            {
                switch (columnName) 
                {
                    case nameof(TruckModel):
                        if (string.IsNullOrWhiteSpace(TruckModel))
                        {
                            return "Необходимо указать модель тягача\n";
                        }
                        break;
                    case nameof(TrailerModel):
                        if (string.IsNullOrWhiteSpace(TruckModel))
                        {
                            return "Необходимо указать модель прицепа\n";
                        }
                        break;
                    case nameof(TruckNumber):
                        return ModelsValidator.IsTruckNumberValid(TruckNumber);
                        break;
                    case nameof(TrailerNumber):
                        return ModelsValidator.IsTrailerNumberValid(TrailerNumber);
                        break;

                }

                return string.Empty;
            }
        }

        public Guid Id { get; set; }
        public CarrierDto Carrier { get; set; }
        public string TruckModel { get; set; }
        public string TruckNumber { get; set; }
        public string TrailerModel { get; set; }
        public string TrailerNumber { get; set; }

        public virtual string Error => this[nameof(TruckModel)];
    }
}
