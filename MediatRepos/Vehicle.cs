using DTOs;
using MediatorServices;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq.Expressions;

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

    public class GetFilterVehicleService : GetFilterModelService<Vehicle>
    {
        public GetFilterVehicleService(IRepository repository, ILogger<GetFilterModelService<Vehicle>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Expression<Func<Vehicle, bool>> filter)
        {
            IEnumerable<Vehicle> vehicles = await _repository.Get<Vehicle>(filter,
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

        protected override Expression<Func<Vehicle, bool>> GetFilter(string property, params object[] parameters)
        {
            Expression<Func<Vehicle, bool>> filter = null;
            switch (property)
            {
                case nameof(VehicleDto.Carrier):
                    Guid guid = (Guid)parameters[0];
                    filter = d => d.CarrierId == guid;
                    break;
                default:
                    string formattedName = parameters[0].ToString().Replace(" ", "").Replace("/", "").ToLower();
                    filter = t => (t.TruckNumber + t.TrailerNumber).Replace("/", "").Replace(" ", "").ToLower().Contains(formattedName);
                    break;
            }

            return filter;
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
                TrailerNumber = dto.TrailerNumber,
                CarrierId = dto?.Carrier?.Id
            };

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

            if (vehicle.CarrierId != dto.Carrier?.Id) 
            {
                IEnumerable<Driver> drivers = await _repository.Get<Driver>(d => d.VehicleId == dto.Id);
                drivers = drivers.ToList();

                foreach (Driver driver in drivers)
                {
                    if (driver.CarrierId != dto.Carrier.Id)
                    {
                        driver.Vehicle = null;
                        driver.VehicleId = null;

                        await _repository.Update(driver);
                    }
                }
            }

            vehicle.CarrierId = dto.Carrier?.Id;

            return await _repository.Update(vehicle);
        }
    }
}
