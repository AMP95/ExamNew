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

    public class GetFilteredTruckService : GetFilteredModelService<Truck>
    {
        public GetFilteredTruckService(IRepository repository, ILogger<GetIdModelService<Truck>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Expression<Func<Truck, bool>> filter)
        {
            IEnumerable<Truck> trucks = await _repository.Get(filter,null, "Carrier");
            List<TruckDto> dtos = new List<TruckDto>();

            foreach (var truck in trucks)
            {
                TruckDto dto = new TruckDto()
                {
                    Id = truck.Id,
                    Model = truck.Model,
                    Number = truck.Number,
                };
                if (truck.Carrier != null)
                {
                    dto.Carrier = new CarrierDto()
                    {
                        Id = truck.Carrier.Id,
                        Name = truck.Carrier.Name,
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
