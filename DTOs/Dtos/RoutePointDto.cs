using System.ComponentModel;

namespace DTOs
{
    public class RoutePointDto : IDataErrorInfo
    {
        public string this[string columnName] 
        {
            get 
            {
                switch (columnName) 
                {
                    case nameof(Route):
                        if (string.IsNullOrWhiteSpace(Route)) 
                        {
                            return "Необходимо указать маршрут\n";
                        }
                        break;
                    case nameof(Address):
                        if (string.IsNullOrWhiteSpace(Address))
                        {
                            return "Необходимо указать адрес\n";
                        }
                        break;
                    case nameof(Phones):
                        return ModelsValidator.IsPhonesValid(Phones);
                        
                }
                return string.Empty;
            }
        }

        public Guid Id { get; set; }
        public string Route { get; set; }
        public string Address { get; set; }
        public LoadingSide Side { get; set; }
        public LoadPointType Type { get; set; }
        public List<string> Phones { get; set; }

        public string Error => this[nameof(Route)] + this[nameof(Address)] + this[nameof(Phones)];
    }
}
