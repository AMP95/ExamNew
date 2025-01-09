using DTOs;
using MediatorServices;
using Microsoft.Extensions.Logging;
using Models;

namespace MediatRepos
{
    public class GetIdVehicleService : GetIdModelService<Vehicle>
    {
        public GetIdVehicleService(IRepository repository, ILogger<GetIdModelService<Vehicle>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            IEnumerable<Vehicle> vehicles = await _repository.Get<Vehicle>(t => t.Id == id, null, "Carrier");
            VehicleDto dto = null;
            if (vehicles.Any()) 
            {
                Vehicle vehicle = vehicles.First();
                dto = new VehicleDto() 
                {
                    Id = vehicle.Id,
                    TruckModel = vehicle.TruckModel,
                    TruckNumber = vehicle.TruckNumber,
                    TrailerModel = vehicle.TrailerModel,
                    TrailerNumber = vehicle.TrailerNumber,
                };
                if (vehicle.CarrierId != null) 
                {
                    dto.Carrier = new CarrierDto()
                    {
                        Id = vehicle.Carrier.Id,
                        Name = vehicle.Carrier.Name,
                    };
                }
            }
            return dto;
        }
    }

    public class GetMainIdVehicleService : GetMainIdModelService<Vehicle>
    {
        public GetMainIdVehicleService(IRepository repository, ILogger<GetMainIdModelService<Vehicle>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            IEnumerable<Vehicle> vehicles = await _repository.Get<Vehicle>(t => t.CarrierId == id, 
                                                                           q => q.OrderBy(t => $"{t.TruckModel} {t.TruckNumber} {t.TrailerModel} {t.TrailerNumber}"),
                                                                           "Carrier");
            List<VehicleDto> dtos = new List<VehicleDto>();

            foreach (var vehicle in vehicles)
            {
                VehicleDto dto = new VehicleDto()
                {
                    Id = vehicle.Id,
                    TruckModel = vehicle.TruckModel,
                    TruckNumber = vehicle.TruckNumber,
                    TrailerModel = vehicle.TrailerModel,
                    TrailerNumber = vehicle.TrailerNumber,
                };

                if (vehicle.CarrierId != null)
                {
                    dto.Carrier = new CarrierDto()
                    {
                        Id = vehicle.Carrier.Id,
                        Name = vehicle.Carrier.Name
                    };
                }

                dtos.Add(dto);
            }

            return dtos;
        }
    }

    public class GetRangeVehicleService : GetRangeModelService<Vehicle>
    {
        public GetRangeVehicleService(IRepository repository, ILogger<GetRangeModelService<Vehicle>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(int start, int end)
        {
            IEnumerable<Vehicle> vehicles = await _repository.GetRange<Vehicle>(start, end, 
                                                                                q => q.OrderBy(t => t.TruckModel).ThenBy(t => t.TruckNumber).ThenBy(t => t.TrailerNumber),
                                                                                "Carrier");
            List<VehicleDto> dtos = new List<VehicleDto>();

            foreach (var vehicle in vehicles)
            {
                VehicleDto dto = new VehicleDto()
                {
                    Id = vehicle.Id,
                    TruckModel = vehicle.TruckModel,
                    TruckNumber = vehicle.TruckNumber,
                    TrailerModel = vehicle.TrailerModel,
                    TrailerNumber = vehicle.TrailerNumber,
                };

                if (vehicle.Carrier != null)
                {
                    dto.Carrier = new CarrierDto()
                    {
                        Id = vehicle.Carrier.Id,
                        Name = vehicle.Carrier.Name
                    };
                }

                dtos.Add(dto);
            }

            return dtos;
        }
    }

    public class SearchVehicleService : SearchModelService<Vehicle>
    {
        public SearchVehicleService(IRepository repository, ILogger<SearchModelService<Vehicle>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(string name)
        {
            string formattedName = name.Trim(" /_-".ToArray()).ToLower();

            IEnumerable<Vehicle> vehicles = await _repository.Get<Vehicle>(t => $"{t.TruckModel}{t.TruckNumber}{t.TrailerModel}{t.TrailerNumber}".Trim(" /_-".ToArray()).ToLower().Contains(formattedName),
                                                                           q => q.OrderBy(t => t.TruckModel).ThenBy(t => t.TruckNumber).ThenBy(t => t.TrailerNumber), 
                                                                           "Carrier");
            List<VehicleDto> dtos = new List<VehicleDto>();

            foreach (var vehicle in vehicles)
            {
                VehicleDto dto = new VehicleDto()
                {
                    Id = vehicle.Id,
                    TruckModel = vehicle.TruckModel,
                    TruckNumber = vehicle.TruckNumber,
                    TrailerNumber = vehicle.TrailerNumber,
                    TrailerModel = vehicle.TrailerModel
                };

                if (vehicle.Carrier != null)
                {
                    dto.Carrier = new CarrierDto()
                    {
                        Id = vehicle.Carrier.Id,
                        Name = vehicle.Carrier.Name
                    };
                }

                dtos.Add(dto);
            }

            return dtos;
        }
    }

    public class DeleteVehicleService : DeleteModelService<Vehicle>
    {
        public DeleteVehicleService(IRepository repository) : base(repository)
        {
        }
    }

    public class AddVehicleService : AddModelService<VehicleDto>
    {
        public AddVehicleService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(VehicleDto dto)
        {
            Vehicle truck = new Vehicle()
            {
                TruckModel = dto.TruckModel,
                TruckNumber = dto.TruckNumber,
                TrailerModel = dto.TrailerModel,
                TrailerNumber = dto.TrailerNumber
            };

            if (dto.Carrier != null)
            {
                Carrier carrier = await _repository.GetById<Carrier>(dto.Carrier.Id);
                truck.Carrier = carrier;
            }

            return await _repository.Update(truck);
        }
    }

    public class UpdateVehicleService : UpdateModelService<VehicleDto>
    {
        public UpdateVehicleService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(VehicleDto dto)
        {
            Vehicle vehicle = await _repository.GetById<Vehicle>(dto.Id);

            vehicle.TruckModel = dto.TruckModel;
            vehicle.TruckNumber = dto.TruckNumber;
            vehicle.TrailerModel = dto.TrailerModel;
            vehicle.TrailerNumber = dto.TrailerNumber;

            if (dto.Carrier != null)
            {
                Carrier carrier = await _repository.GetById<Carrier>(dto.Carrier.Id);
                vehicle.Carrier = carrier;
            }
            else
            {
                vehicle.Carrier = null;
                vehicle.CarrierId = null;
            }

            IEnumerable<Driver> drivers = await _repository.Get<Driver>(d => d.VehicleId == dto.Id);

            foreach (Driver driver in drivers)
            {
                if (driver.CarrierId != vehicle.CarrierId)
                {
                    driver.Vehicle = null;
                    driver.VehicleId = null;

                    await _repository.Update(driver);
                }
            }

            return await _repository.Update(vehicle);
        }
    }
}
