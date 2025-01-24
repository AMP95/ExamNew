using DTOs;
using MediatorServices;
using MediatorServices.Abstract;
using MediatR;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq.Expressions;

namespace MediatRepos
{
    public class GetIdVehicleService : GetIdModelService<VehicleDto>
    {
        public GetIdVehicleService(IRepository repository, ILogger<GetIdModelService<VehicleDto>> logger) : base(repository, logger)
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

    public class GetRangeVehicleService : GetRangeModelService<VehicleDto>
    {
        public GetRangeVehicleService(IRepository repository, ILogger<GetRangeModelService<VehicleDto>> logger) : base(repository, logger)
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

    public class GetFilterVehicleService : IRequestHandler<GetFilter<VehicleDto>, object>
    {
        protected IRepository _repository;
        protected ILogger<GetFilterVehicleService> _logger;

        public GetFilterVehicleService(IRepository repository, ILogger<GetFilterVehicleService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<object> Handle(GetFilter<VehicleDto> request, CancellationToken cancellationToken)
        {
            Expression<Func<Vehicle, bool>> filter = GetFilter(request.PropertyName, request.Params);
            return await Get(filter);
        }

        protected async Task<object> Get(Expression<Func<Vehicle, bool>> filter)
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

        protected Expression<Func<Vehicle, bool>> GetFilter(string property, params object[] parameters)
        {
            Expression<Func<Vehicle, bool>> filter = null;

            try
            {
                switch (property)
                {
                    case nameof(VehicleDto.Carrier):
                        if (parameters[0] == null)
                        {
                            filter = d => d.CarrierId == null;
                        }
                        else
                        {
                            string carname = parameters[0].ToString().ToLower();
                            filter = d => d.Carrier.Name.ToLower().Contains(carname);
                        }
                        break;
                    case "CarrierId":
                        if (parameters == null || !parameters.Any() || parameters[0] == null)
                        {
                            filter = d => d.CarrierId == null;
                        }
                        else
                        {
                            Guid.TryParse(parameters[0].ToString(), out Guid id);

                            if (id == Guid.Empty)
                            {
                                filter = d => d.CarrierId == null;
                            }
                            else
                            {
                                filter = d => d.CarrierId == id;
                            }
                        }
                        break;
                    default:
                        string formattedName = parameters[0].ToString().Replace(" ", "").Replace("/", "").ToLower();
                        filter = t => (t.TruckNumber + t.TrailerNumber).Replace("/", "").Replace(" ", "").ToLower().Contains(formattedName);
                        break;
                }
            }
            catch (Exception ex)
            {
                filter = v => false;
                _logger.LogError(ex, ex.Message);
            }

            return filter;
        }
    }

    public class DeleteVehicleService : IRequestHandler<Delete<VehicleDto>, bool>
    {
        private IRepository _repository;
        private IFileManager _fileManager;

        public DeleteVehicleService(IRepository repository, IFileManager fileManager)
        {
            _repository = repository;
            _fileManager = fileManager;
        }

        public async Task<bool> Handle(Delete<VehicleDto> request, CancellationToken cancellationToken)
        {
            bool result = await _repository.Remove<Vehicle>(request.Id);

            IEnumerable<Models.Sub.File> files = await _repository.Get<Models.Sub.File>(f => f.EntityType == nameof(Vehicle) && f.EntityId == request.Id);

            if (files.Any())
            {
                string catalog = Path.GetFileName(Path.GetDirectoryName(files.First().FullFilePath));

                await _fileManager.RemoveAllFiles(nameof(Vehicle), catalog);
            }

            return result;
        }
    }

    public class AddVehicleService : AddModelService<VehicleDto>
    {
        public AddVehicleService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<Guid> Add(VehicleDto dto)
        {
            Vehicle truck = new Vehicle()
            {
                TruckModel = dto.TruckModel,
                TruckNumber = dto.TruckNumber,
                TrailerModel = dto.TrailerModel,
                TrailerNumber = dto.TrailerNumber
            };

            if (dto.Carrier != null && dto.Carrier.Id != Guid.Empty) 
            { 
                truck.CarrierId = dto.Carrier.Id;
            }

            return await _repository.Add(truck);
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

            if (dto.Carrier != null && dto.Carrier.Id != Guid.Empty)
            {
                vehicle.CarrierId = dto.Carrier.Id;
            }
            else 
            {
                vehicle.CarrierId = null;
            }

            return await _repository.Update(vehicle);
        }
    }

}
