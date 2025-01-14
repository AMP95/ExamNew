using DTOs;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq.Expressions;

namespace MediatorServices
{
    public class GetIdDriverService : GetIdModelService<Driver>
    {
        public GetIdDriverService(IRepository repository, ILogger<GetIdModelService<Driver>> logger) : base(repository, logger)
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

    public class GetMainIdDriverService : GetMainIdModelService<Driver>
    {
        public GetMainIdDriverService(IRepository repository, ILogger<GetMainIdModelService<Driver>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            IEnumerable<Driver> drivers = await _repository.Get<Driver>(d => d.CarrierId == id, 
                                                                        q => q.OrderBy(d => d.FamilyName).ThenBy(d => d.Name).ThenBy(d => d.FatherName),
                                                                        "Carrier,Vehicle");
            List<DriverDto> dtos = new List<DriverDto>();

            foreach (var driver in drivers)
            {
                DriverDto dto = new DriverDto()
                {
                    Id = driver.Id,
                    Name = $"{driver.FamilyName} {driver.Name} {driver.FamilyName}"
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
                        TrailerNumber = driver.Vehicle.TrailerNumber,
                        TrailerModel = driver.Vehicle.TrailerModel,
                    };
                }

                dtos.Add(dto);
            }

            return dtos;
        }
    }

    public class SearchDriverService : SearchModelService<Driver>
    {
        public SearchDriverService(IRepository repository, ILogger<SearchModelService<Driver>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(string name)
        {
            IEnumerable<Driver> drivers = await _repository.Get<Driver>(d => d.FamilyName.ToLower().Contains(name.ToLower()), 
                                                                        q => q.OrderBy(d => d.FamilyName).ThenBy(d => d.Name).ThenBy(d => d.FatherName), 
                                                                        "Carrier,Vehicle");
            List<DriverDto> dtos = new List<DriverDto>();

            foreach (var driver in drivers)
            {
                DriverDto dto = new DriverDto()
                {
                    Id = driver.Id,
                    Name = $"{driver.FamilyName} {driver.Name} {driver.FamilyName}"
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
    }

    public class GetRangeDriverService : GetRangeModelService<Driver>
    {
        public GetRangeDriverService(IRepository repository, ILogger<GetRangeModelService<Driver>> logger) : base(repository, logger)
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
                    Name = $"{driver.FamilyName} {driver.Name} {driver.FamilyName}"
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

    public class DeleteDriverService : DeleteModelService<Driver>
    {
        public DeleteDriverService(IRepository repository) : base(repository)
        {
        }
    }

    public class AddDriverService : AddModelService<DriverDto>
    {
        public AddDriverService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(DriverDto dto)
        {
            string[] name = dto.Name.Split(' ');

            Driver driver = new Driver()
            {
                FamilyName = name[0],
                Name = name[1],
                FatherName = name.Length > 2 ? name[2] : string.Empty,
                DateOfBirth = dto.BirthDate,
                PassportDateOfIssue = dto.PassportDateOfIssue,
                PassportIssuer = dto.PassportIssuer,
                PassportSerial = dto.PassportSerial,
                Phones = string.Join(';',dto.Phones)
            };

            if (dto.Vehicle != null) 
            {
                Vehicle vehicle = await _repository.GetById<Vehicle>(dto.Vehicle.Id);
                driver.Vehicle = vehicle;
            }

            if (dto.Carrier != null) 
            {
                Carrier carrier = await _repository.GetById<Carrier>(dto.Carrier.Id);
                driver.Carrier = carrier;
            }

            return await _repository.Update(driver);
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

            if (dto.Carrier == null)
            {
                driver.Carrier = null;
                driver.CarrierId = null;
            }
            else
            {
                Carrier carrier = await _repository.GetById<Carrier>(dto.Carrier.Id);
                driver.Carrier = carrier;
            }

            if (driver.Vehicle != null && driver.Vehicle.CarrierId != driver.CarrierId)
            {
                driver.Vehicle = null;
                driver.VehicleId = null;
            }

            if (dto.Vehicle == null)
            {
                driver.Vehicle = null;
                driver.VehicleId = null;
            }
            else
            {
                Vehicle truck = await _repository.GetById<Vehicle>(dto.Vehicle.Id);
                driver.Vehicle = truck;
            }


            return await _repository.Update(driver);
        }
    }
}
