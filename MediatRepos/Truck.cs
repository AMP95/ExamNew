using DTOs;
using MediatorServices;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq.Expressions;

namespace MediatRepos
{
    public class GetIdTruckService : GetIdModelService<Truck>
    {
        public GetIdTruckService(IRepository repository, ILogger<GetIdModelService<Truck>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            IEnumerable<Truck> trucks = await _repository.Get<Truck>(t => t.Id == id, null, "Carrier");
            TruckDto dto = null;
            if (trucks.Any()) 
            {
                Truck truck = trucks.First();
                dto = new TruckDto() 
                {
                    Id = truck.Id,
                    Model = truck.Model,
                    Number = truck.Number
                };
                if (truck.Carrier != null) 
                {
                    dto.Carrier = new CarrierDto()
                    {
                        Id = truck.Carrier.Id,
                        Name = truck.Carrier.Name,
                    };
                }
            }
            return dto;
        }
    }

    public class GetMainIdTrailerService : GetMainIdModelService<Truck>
    {
        public GetMainIdTrailerService(IRepository repository, ILogger<GetMainIdModelService<Truck>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            IEnumerable<Truck> trailers = await _repository.Get<Truck>(t => t.CarrierId == id);
            List<TruckDto> dtos = new List<TruckDto>();

            foreach (var trailer in trailers)
            {
                TruckDto dto = new TruckDto()
                {
                    Id = trailer.Id,
                    Model = trailer.Model,
                    Number = trailer.Number
                };

                if (trailer.Carrier != null)
                {
                    dto.Carrier = new CarrierDto()
                    {
                        Id = trailer.Carrier.Id,
                        Name = trailer.Carrier.Name
                    };
                }

                dtos.Add(dto);
            }

            return dtos;
        }
    }

    public class GetRangeTrailerService : GetRangeModelService<Truck>
    {
        public GetRangeTrailerService(IRepository repository, ILogger<GetRangeModelService<Truck>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(int start, int end)
        {
            IEnumerable<Truck> trailers = await _repository.GetRange<Truck>(start, end);
            List<TruckDto> dtos = new List<TruckDto>();

            foreach (var trailer in trailers)
            {
                TruckDto dto = new TruckDto()
                {
                    Id = trailer.Id,
                    Model = trailer.Model,
                    Number = trailer.Number
                };

                if (trailer.Carrier != null)
                {
                    dto.Carrier = new CarrierDto()
                    {
                        Id = trailer.Carrier.Id,
                        Name = trailer.Carrier.Name
                    };
                }

                dtos.Add(dto);
            }

            return dtos;
        }
    }

    public class SearchTrailerService : SearchModelService<Truck>
    {
        public SearchTrailerService(IRepository repository, ILogger<SearchModelService<Truck>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(string name)
        {
            IEnumerable<Truck> trailers = await _repository.Get<Truck>(t => $"{t.Model}{t.Number.Replace(" ", "")}".ToLower().Contains(name.Replace(" ", "").ToLower()), null, "Carrier");
            List<TruckDto> dtos = new List<TruckDto>();

            foreach (var trailer in trailers)
            {
                TruckDto dto = new TruckDto()
                {
                    Id = trailer.Id,
                    Model = trailer.Model,
                    Number = trailer.Number
                };

                if (trailer.Carrier != null)
                {
                    dto.Carrier = new CarrierDto()
                    {
                        Id = trailer.Carrier.Id,
                        Name = trailer.Carrier.Name
                    };
                }

                dtos.Add(dto);
            }

            return dtos;
        }
    }

    public class DeleteTruckService : DeleteModelService<Truck>
    {
        public DeleteTruckService(IRepository repository) : base(repository)
        {
        }
    }

    public class AddTruckService : AddModelService<TruckDto>
    {
        public AddTruckService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(TruckDto dto)
        {
            Truck truck = new Truck()
            {
                Id = Guid.NewGuid(),
                Model = dto.Model,
                Number = dto.Number,
            };

            if (dto.Carrier != null)
            {
                Carrier carrier = await _repository.GetById<Carrier>(dto.Carrier.Id);
                truck.Carrier = carrier;
            }

            return await _repository.Update(truck);
        }
    }

    public class UpdateTruckService : UpdateModelService<TruckDto>
    {
        public UpdateTruckService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(TruckDto dto)
        {
            Truck truck = await _repository.GetById<Truck>(dto.Id);

            truck.Model = dto.Model;
            truck.Number = dto.Number;

            if (dto.Carrier != null)
            {
                Carrier carrier = await _repository.GetById<Carrier>(dto.Carrier.Id);
                truck.Carrier = carrier;
            }
            else
            {
                truck.Carrier = null;
                truck.CarrierId = null;
            }

            IEnumerable<Driver> drivers = await _repository.Get<Driver>(d => d.TruckId == dto.Id);

            foreach (Driver driver in drivers)
            {
                if (driver.CarrierId != truck.CarrierId)
                {
                    driver.Truck = null;
                    driver.TruckId = null;

                    await _repository.Update(driver);
                }
            }

            return await _repository.Update(truck);
        }
    }
}
