using DTOs;
using DTOs.Dtos;
using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq.Expressions;
using Utilities.Interfaces;

namespace MediatorServices
{
    public class GetIdDriverService : IRequestHandler<GetId<DriverDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public GetIdDriverService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(GetId<DriverDto> request, CancellationToken cancellationToken)
        {
            IEnumerable<Driver> drivers = await _repository.Get<Driver>(d => d.Id == request.Id, null, "Vehicle");

            if (drivers.Any())
            {
                Driver driver = drivers.First();

                DriverDto dto = new DriverDto()
                {
                    Id = driver.Id,
                    Name = $"{driver.FamilyName} {driver.Name} {driver.FamilyName}",
                    BirthDate = driver.DateOfBirth,
                    Phones = driver.Phones.Split(';').ToList(),
                    PassportDateOfIssue = driver.PassportDateOfIssue,
                    PassportIssuer = driver.PassportIssuer,
                    PassportSerial = driver.PassportSerial,
                };
                if (driver.CarrierId != null)
                {
                    IEnumerable<Carrier> carriers = await _repository.Get<Carrier>(c => c.Id == driver.CarrierId, null, "Vehicles");
                    Carrier carrier = carriers.First();

                    dto.Carrier = new CarrierDto()
                    {
                        Id = carrier.Id,
                        Name = carrier.Name,
                        Vat = (VAT)carrier.Vat,
                        Address = carrier.Address,
                        InnKpp = carrier.InnKpp,
                        Emails = carrier.Emails.Split(";").ToList(),
                        Phones = carrier.Phones.Split(";").ToList(),
                        Vehicles = carrier.Vehicles.Select(t => new VehicleDto()
                        {
                            Id = t.Id,
                            TruckNumber = t.TruckNumber,
                            TruckModel = t.TruckModel,
                            TrailerModel = t.TrailerModel,
                            TrailerNumber = t.TrailerNumber,
                        }).ToList(),
                    };
                }

                if (driver.VehicleId != null)
                {
                    dto.Vehicle = new VehicleDto()
                    {
                        Id = driver.Vehicle.Id,
                        TruckModel = driver.Vehicle.TruckModel,
                        TruckNumber = driver.Vehicle.TruckNumber,
                        TrailerModel = driver.Vehicle.TrailerModel,
                        TrailerNumber = driver.Vehicle.TrailerNumber,
                    };
                    if (driver.CarrierId != null)
                    {
                        dto.Vehicle.Carrier = new CarrierDto() { Id = driver.CarrierId.Value };
                    }
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
                    ErrorMessage = "Водитель не найден"
                };
            }
        }
    }

    public class GetRangeDriverService : IRequestHandler<GetRange<DriverDto>, IServiceResult<object>>
    {
        private IRepository _repository;
        public GetRangeDriverService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(GetRange<DriverDto> request, CancellationToken cancellationToken)
        {
            IEnumerable<Driver> drivers = await _repository.GetRange<Driver>(request.Start, request.End, 
                                                                             q => q.OrderBy(d => d.FamilyName).ThenBy(d => d.Name).ThenBy(d => d.FatherName), 
                                                                             "Carrier,Vehicle");
            List<DriverDto> dtos = new List<DriverDto>();

            foreach (var driver in drivers)
            {
                DriverDto dto = new DriverDto()
                {
                    Id = driver.Id,
                    Name = $"{driver.FamilyName} {driver.Name} {driver.FatherName}"
                };

                if (driver.Carrier != null)
                {
                    dto.Carrier = new CarrierDto()
                    {
                        Id = driver.Carrier.Id,
                        Name = driver.Carrier.Name
                    };
                }

                if (driver.VehicleId != null)
                {
                    dto.Vehicle = new VehicleDto()
                    {
                        Id = driver.Vehicle.Id,
                        TruckModel = driver.Vehicle.TruckModel,
                        TruckNumber = driver.Vehicle.TruckNumber,
                        TrailerModel = driver.Vehicle.TrailerModel,
                        TrailerNumber = driver.Vehicle.TrailerNumber,
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

    public class GetFilterDriverService : IRequestHandler<GetFilter<DriverDto>, IServiceResult<object>>
    {
        protected IRepository _repository;
        protected ILogger<GetFilterDriverService> _logger;

        public GetFilterDriverService(IRepository repository, ILogger<GetFilterDriverService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IServiceResult<object>> Handle(GetFilter<DriverDto> request, CancellationToken cancellationToken)
        {
            Expression<Func<Driver, bool>> filter = GetFilter(request.PropertyName, request.Params);

            IEnumerable<Driver> drivers = await _repository.Get<Driver>(filter,
                                                                        q => q.OrderBy(d => d.FamilyName).ThenBy(d => d.Name).ThenBy(d => d.FatherName),
                                                                        "Carrier,Vehicle");
            List<DriverDto> dtos = new List<DriverDto>();

            foreach (var driver in drivers)
            {
                DriverDto dto = new DriverDto()
                {
                    Id = driver.Id,
                    Name = $"{driver.FamilyName} {driver.Name} {driver.FatherName}"
                };

                if (driver.Carrier != null)
                {
                    dto.Carrier = new CarrierDto()
                    {
                        Id = driver.Carrier.Id,
                        Name = driver.Carrier.Name,
                        Vat = (VAT)driver.Carrier.Vat,
                    };
                }

                if (driver.VehicleId != null)
                {
                    dto.Vehicle = new VehicleDto()
                    {
                        Id = driver.Vehicle.Id,
                        TruckModel = driver.Vehicle.TruckModel,
                        TruckNumber = driver.Vehicle.TruckNumber,
                        TrailerModel = driver.Vehicle.TrailerModel,
                        TrailerNumber = driver.Vehicle.TrailerNumber,
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

        protected Expression<Func<Driver, bool>> GetFilter(string property, params object[] parameters)
        {
            Expression<Func<Driver, bool>> filter = null;

            try
            {
                switch (property)
                {
                    case nameof(DriverDto.Carrier):
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
                        string name = parameters[0].ToString().ToLower();
                        filter = d => (d.FamilyName + d.Name + d.FatherName).ToLower().Contains(name);
                        break;
                }
            }
            catch (Exception ex)
            {
                filter = d => false;
                _logger.LogError(ex, ex.Message);
            }

            return filter;
        }
    }

    public class DeleteDriverService : IRequestHandler<Delete<DriverDto>, IServiceResult<object>>
    {
        private IRepository _repository;
        private IFileManager _fileManager;

        public DeleteDriverService(IRepository repository, IFileManager fileManager)
        {
            _repository = repository;
            _fileManager = fileManager;
        }

        public async Task<IServiceResult<object>> Handle(Delete<DriverDto> request, CancellationToken cancellationToken)
        {
            if (await _repository.Remove<Driver>(request.Id))
            {
                IEnumerable<Models.Sub.File> files = await _repository.Get<Models.Sub.File>(f => f.EntityType == nameof(Driver) && f.EntityId == request.Id);

                if (files.Any())
                {
                    string catalog = Path.GetFileName(Path.GetDirectoryName(files.First().FullFilePath));

                    await _fileManager.RemoveAllFiles(nameof(Driver), catalog);
                }

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
                    ErrorMessage = "Не удалось удалить водителя",
                    Result = false
                };
            }
        }
    }

    public class AddDriverService : IRequestHandler<Add<DriverDto>, IServiceResult<object>>
    {
        private IRepository _repository;
        public AddDriverService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(Add<DriverDto> request, CancellationToken cancellationToken)
        {
            DriverDto dto = request.Value;

            string[] name = dto.Name.Split(' ');

            Driver driver = new Driver()
            {
                VehicleId = dto.Vehicle?.Id,
                FamilyName = name[0],
                Name = name[1],
                FatherName = name.Length > 2 ? name[2] : string.Empty,
                DateOfBirth = dto.BirthDate,
                PassportDateOfIssue = dto.PassportDateOfIssue,
                PassportIssuer = dto.PassportIssuer,
                PassportSerial = dto.PassportSerial,
                Phones = string.Join(';', dto.Phones)
            };

            if (dto.Carrier != null && dto.Carrier.Id != Guid.Empty)
            {
                driver.CarrierId = dto.Carrier.Id;
            }

            if (dto.Vehicle != null && dto.Vehicle.Id != Guid.Empty)
            {
                driver.VehicleId = dto.Vehicle.Id;
            }

            Guid id = await _repository.Add(driver);

            if (id == Guid.Empty)
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Не удалось сохранить водителя",
                    Result = Guid.Empty
                };
            }
            else 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = true,
                    Result = id
                };
            }
        }
    }

    public class UpdateDriverService : IRequestHandler<Update<DriverDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public UpdateDriverService(IRepository repository) 
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(Update<DriverDto> request, CancellationToken cancellationToken)
        {
            DriverDto dto = request.Value;

            string[] name = dto.Name.Split(' ');

            Driver driver = await _repository.GetById<Driver>(dto.Id);

            if (driver == null) 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Водитель не найден",
                    Result = false
                };
            }

            driver.FamilyName = name[0];
            driver.Name = name[1];
            driver.FatherName = name.Length > 2 ? name[2] : string.Empty;
            driver.DateOfBirth = dto.BirthDate;
            driver.Phones = string.Join(';', dto.Phones);
            driver.PassportDateOfIssue = dto.PassportDateOfIssue;
            driver.PassportIssuer = dto.PassportIssuer;
            driver.PassportSerial = dto.PassportSerial;

            if (dto.Carrier != null && dto.Carrier.Id != Guid.Empty)
            {
                driver.CarrierId = dto.Carrier.Id;
            }

            if (dto.Vehicle != null && dto.Vehicle.Id != Guid.Empty)
            {
                if (dto.Vehicle.Carrier?.Id != dto.Carrier?.Id)
                {
                    driver.VehicleId = null;
                }
                else
                {
                    driver.VehicleId = dto.Vehicle.Id;
                }
            }
            else
            {
                driver.VehicleId = null;
            }

            if (await _repository.Update(driver))
            {
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
                    ErrorMessage = "Не удалось обновить водителя",
                    Result = false
                };
            }
        }
    }
}
