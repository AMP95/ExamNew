using System.ComponentModel;

namespace DTOs
{
    public class VehicleDto : IDataErrorInfo
    {
        public virtual string this[string columnName] 
        {
            get 
            { 
                if (columnName == nameof(Model))
                {
                    if (string.IsNullOrWhiteSpace(Model)) 
                    {
                        return "Необходимо указать модель\n";
                    }
                }
                return string.Empty;
            }
        }

        public Guid Id { get; set; }
        public CarrierDto Carrier { get; set; }
        public string Model { get; set; }
        public string Number { get; set; }

        public virtual string Error => this[nameof(Model)];
    }
    public class TruckDto : VehicleDto
    {
        public override string this[string columnName]  
        {
            get
            {
                if (columnName == nameof(Number))
                {
                    return ModelsValidator.IsTruckNumberValid(Number);
                }
                else 
                { 
                    return base.Error;
                }
            }
        }
        public override string Error => base.Error + this[nameof(Number)];

    }
    public class TrailerDto : VehicleDto
    {
        public override string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Number))
                {
                    return ModelsValidator.IsTrailerNumberValid(Number);
                }
                else
                {
                    return base.Error;
                }
            }
        }
        public override string Error => base.Error + this[nameof(Number)];
    }
}
