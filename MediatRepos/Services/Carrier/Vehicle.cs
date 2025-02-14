using DTOs;
using MediatorServices;
using MediatR;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq.Expressions;
using Utilities.Interfaces;

namespace MediatRepos
{
    public class GetIdVehicleService : IRequestHandler<GetId<VehicleDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public GetIdVehicleService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(GetId<VehicleDto> request, CancellationToken cancellationToken)
        {
            IEnumerable<Vehicle> vehicles = await _repository.Get<Vehicle>(t => t.Id == request.Id, null, "Carrier");

            if (vehicles.Any())
            {
                Vehicle vehicle = vehicles.First();

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
                        Name = vehicle.Carrier.Name,
                    };
                }

                return new MediatorServiceResult()
                {
                    IsSuccess = true,
                    Result = dto
                };
            }
            else 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "ТС не найдено"
                };
            }
        }
    }

    public class GetRangeVehicleService : IRequestHandler<GetRange<VehicleDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public GetRangeVehicleService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(GetRange<VehicleDto> request, CancellationToken cancellationToken)
        {
            IEnumerable<Vehicle> vehicles = await _repository.GetRange<Vehicle>(request.Start, request.End,
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

            return new MediatorServiceResult()
            {
                IsSuccess = true,
                Result = dtos
            };
        }
    }

    public class GetFilterVehicleService : IRequestHandler<GetFilter<VehicleDto>, IServiceResult<object>>
    {
        protected IRepository _repository;
        protected ILogger<GetFilterVehicleService> _logger;

        public GetFilterVehicleService(IRepository repository, ILogger<GetFilterVehicleService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IServiceResult<object>> Handle(GetFilter<VehicleDto> request, CancellationToken cancellationToken)
        {
            Expression<Func<Vehicle, bool>> filter = GetFilter(request.PropertyName, request.Params);

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

            return new MediatorServiceResult()
            {
                IsSuccess = true,
                Result = dtos
            };
        }

        private Expression<Func<Vehicle, bool>> GetFilter(string property, params object[] parameters)
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

    public class DeleteVehicleService : IRequestHandler<Delete<VehicleDto>, IServiceResult<object>>
    {
        private IRepository _repository;
        private IFileManager _fileManager;

        public DeleteVehicleService(IRepository repository, IFileManager fileManager)
        {
            _repository = repository;
            _fileManager = fileManager;
        }

        public async Task<IServiceResult<object>> Handle(Delete<VehicleDto> request, CancellationToken cancellationToken)
        {
            if (await _repository.Remove<Vehicle>(request.Id))
            {
                IEnumerable<Models.Sub.File> files = await _repository.Get<Models.Sub.File>(f => f.EntityType == nameof(Vehicle) && f.EntityId == request.Id);

                if (files.Any())
                {
                    string catalog = Path.GetFileName(Path.GetDirectoryName(files.First().FullFilePath));

                    await _fileManager.RemoveAllFiles(nameof(Vehicle), catalog);
                }

                return new MediatorServiceResult()
                {
                    IsSuccess = true,
                    Result = true
                };
            }
            else 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "ТС не найдено",
                    Result = false
                };
            }
        }
    }

    public class AddVehicleService : IRequestHandler<Add<VehicleDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public AddVehicleService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(Add<VehicleDto> request, CancellationToken cancellationToken)
        {
            VehicleDto dto = request.Value;

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

            Guid id = await _repository.Add(truck);

            if (id == Guid.Empty)
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Де удалось добавить ТС",
                    Result = Guid.Empty
                };
            }
            else 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = true,
                    Result  = id
                };
            }
        }
    }


    public class UpdateVehicleService : IRequestHandler<Update<VehicleDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public UpdateVehicleService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(Update<VehicleDto> request, CancellationToken cancellationToken)
        {
            VehicleDto dto = request.Value;

            Vehicle vehicle = await _repository.GetById<Vehicle>(dto.Id);

            if (vehicle == null) 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "ТС не найдено",
                    Result = false
                };
            }

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

            if (await _repository.Update(vehicle))
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = true,
                    Result = true,
                };
            }
            else 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Не обновить ТС",
                    Result= false
                };
            }
        }
    }
}
