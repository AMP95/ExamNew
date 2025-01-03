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
            IEnumerable<Driver> drivers = await _repository.Get<Driver>(d => d.Id == id, null, "Truck,Trailer,Carrier");
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
                    PassportDateOfIssue = dto.PassportDateOfIssue,
                    PassportIssuer = dto.PassportIssuer,
                    PassportSerial = dto.PassportSerial,
                };
                if (driver.Carrier != null) 
                {
                    dto.Carrier = new CarrierDto()
                    {
                        Id = driver.Carrier.Id,
                        Name = driver.Carrier.Name,
                        Vat = (VAT)driver.Carrier.Vat,
                        Address = driver.Carrier.Address,
                        InnKpp = driver.Carrier.InnKpp,
                        Emails = driver.Carrier.Emails.Split(";").ToList(),
                        Phones = driver.Carrier.Phones.Split(";").ToList(),
                    };
                }
                if (driver.Truck != null) 
                { 
                    dto.Truck = new TruckDto() 
                    { 
                        Id = driver.Truck.Id,
                        Model = driver.Truck.Model,
                        Number = driver.Truck.Number,
                    };
                }
                if (driver.Trailer != null) 
                {
                    dto.Trailer = new TrailerDto()
                    {
                        Id = driver.Trailer.Id,
                        Number = driver.Trailer.Number,
                        Model = driver.Trailer.Model
                    };
                }
            }
            return dto;
        }
    }

    public class GetFilteredDriverService : GetFilteredModelService<Driver>
    {
        public GetFilteredDriverService(IRepository repository, ILogger<GetIdModelService<Driver>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Expression<Func<Driver, bool>> filter)
        {
            IEnumerable<Driver> drivers = await _repository.Get(filter);
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
                Id = Guid.NewGuid(),
                FamilyName = name[0],
                Name = name[1],
                FatherName = name.Length > 2 ? name[2] : string.Empty,
                DateOfBirth = dto.BirthDate,
                PassportDateOfIssue = dto.PassportDateOfIssue,
                PassportIssuer = dto.PassportIssuer,
                PassportSerial = dto.PassportSerial,
                Phones = string.Join(';',dto.Phones)
            };

            if (dto.Truck != null) 
            {
                Truck truck = await _repository.GetById<Truck>(dto.Truck.Id);
                driver.Truck = truck;
            }

            if (dto.Trailer != null)
            {
                Trailer trailer = await _repository.GetById<Trailer>(dto.Trailer.Id);
                driver.Trailer = trailer;
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

            if (driver.Trailer != null && driver.Trailer.CarrierId != driver.CarrierId)
            {
                driver.Trailer = null;
                driver.TrailerId = null;
            }

            if (driver.Truck != null && driver.Truck.CarrierId != driver.CarrierId)
            {
                driver.Truck = null;
                driver.TruckId = null;
            }

            if (dto.Truck == null)
            {
                driver.Truck = null;
                driver.TruckId = null;
            }
            else
            {
                Truck truck = await _repository.GetById<Truck>(dto.Truck.Id);
                driver.Truck = truck;
            }

            if (dto.Trailer == null)
            {
                driver.Trailer = null;
                driver.TrailerId = null;
            }
            else 
            {
                Trailer trailer = await _repository.GetById<Trailer>(dto.Trailer.Id);
                driver.Trailer = trailer;
            }

            return await _repository.Update(driver);
        }
    }
}
