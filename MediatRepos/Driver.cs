using DTOs;
using MediatorServices.Abstract;
using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using System.Diagnostics;
using System.Linq.Expressions;

namespace MediatorServices
{
    public class GetIdDriverService : GetIdModelService<DriverDto>
    {
        public GetIdDriverService(IRepository repository, ILogger<GetIdModelService<DriverDto>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            IEnumerable<Driver> drivers = await _repository.Get<Driver>(d => d.Id == id, null, "Vehicle");
            DriverDto dto = null;
            if (drivers.Any())
            {
                Driver driver = drivers.First();

                dto = new DriverDto()
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
            }
            return dto;
        }
    }

    public class GetRangeDriverService : GetRangeModelService<DriverDto>
    {
        public GetRangeDriverService(IRepository repository, ILogger<GetRangeModelService<DriverDto>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(int start, int end)
        {
            IEnumerable<Driver> drivers = await _repository.GetRange<Driver>(start, end, q => q.OrderBy(d => d.FamilyName).ThenBy(d => d.Name).ThenBy(d => d.FatherName), "Carrier,Vehicle");
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

            return dtos;
        }
    }

    public class GetFilterDriverService : IRequestHandler<GetFilter<DriverDto>, object>
    {
        protected IRepository _repository;
        protected ILogger<GetFilterDriverService> _logger;

        public GetFilterDriverService(IRepository repository, ILogger<GetFilterDriverService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<object> Handle(GetFilter<DriverDto> request, CancellationToken cancellationToken)
        {
            Expression<Func<Driver, bool>> filter = GetFilter(request.PropertyName, request.Params);
            return await Get(filter);
        }

        protected async Task<object> Get(Expression<Func<Driver, bool>> filter)
        {
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

            return dtos;
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

    public class DeleteDriverService : IRequestHandler<Delete<DriverDto>, bool>
    {
        private IRepository _repository;
        private IFileManager _fileManager;

        public DeleteDriverService(IRepository repository, IFileManager fileManager)
        {
            _repository = repository;
            _fileManager = fileManager;
        }

        public async Task<bool> Handle(Delete<DriverDto> request, CancellationToken cancellationToken)
        {
            bool result = await _repository.Remove<Driver>(request.Id);

            IEnumerable<Models.Sub.File> files = await _repository.Get<Models.Sub.File>(f => f.EntityType == nameof(Driver) && f.EntityId == request.Id);

            if (files.Any())
            {
                string catalog = Path.GetFileName(Path.GetDirectoryName(files.First().FullFilePath));

                await _fileManager.RemoveAllFiles(nameof(Driver), catalog);
            }

            return result;
        }
    }

    public class AddDriverService : AddModelService<DriverDto>
    {
        public AddDriverService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<Guid> Add(DriverDto dto)
        {
            string[] name = dto.Name.Split(' ');

            Driver driver = new Driver()
            {
                CarrierId = dto.Carrier?.Id,
                VehicleId = dto.Vehicle?.Id,
                FamilyName = name[0],
                Name = name[1],
                FatherName = name.Length > 2 ? name[2] : string.Empty,
                DateOfBirth = dto.BirthDate,
                PassportDateOfIssue = dto.PassportDateOfIssue,
                PassportIssuer = dto.PassportIssuer,
                PassportSerial = dto.PassportSerial,
                Phones = string.Join(';',dto.Phones)
            };

            return await _repository.Add(driver);
        }
    }

    public class UpdateDriverService : UpdateModelService<DriverDto>
    {
        public UpdateDriverService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(DriverDto dto)
        {
            string[] name = dto.Name.Split(' ');

            Driver driver = await _repository.GetById<Driver>(dto.Id);

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

            if (dto.Vehicle?.Carrier?.Id != dto.Carrier?.Id)
            {
                driver.VehicleId = null;
            }
            else 
            { 
                driver.VehicleId = dto.Vehicle?.Id;
            }

            return await _repository.Update(driver);
        }
    }
}
