using DTOs;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq.Expressions;

namespace MediatorServices
{
    public class GetIdTrailerService : GetIdModelService<Trailer>
    {
        public GetIdTrailerService(IRepository repository, ILogger<GetIdModelService<Trailer>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            IEnumerable<Trailer> trailers = await _repository.Get<Trailer>(t => t.Id == id, null, "Carrier");
            TruckDto dto = null;
            if (trailers.Any())
            {
                Trailer trailer = trailers.First();
                dto = new TruckDto()
                {
                    Id = trailer.Id,
                    Model = trailer.Model,
                    Number = trailer.Number,
                    Carrier = new CarrierDto() 
                    { 
                        Id = trailer.Carrier.Id,
                        Name = trailer.Carrier.Name
                    }
                };
            }
            return dto;
        }
    }

    public class GetMainIdTrailerService : GetMainIdModelService<Trailer>
    {
        public GetMainIdTrailerService(IRepository repository, ILogger<GetMainIdModelService<Trailer>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            IEnumerable<Trailer> trailers = await _repository.Get<Trailer>(t => t.CarrierId == id);
            List<TrailerDto> dtos = new List<TrailerDto>();

            foreach (var trailer in trailers)
            {
                TrailerDto dto = new TrailerDto()
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

    public class GetRangeTrailerService : GetRangeModelService<Trailer>
    {
        public GetRangeTrailerService(IRepository repository, ILogger<GetRangeModelService<Trailer>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(int start, int end)
        {
            IEnumerable<Trailer> trailers = await _repository.GetRange<Trailer>(start, end);
            List<TrailerDto> dtos = new List<TrailerDto>();

            foreach (var trailer in trailers)
            {
                TrailerDto dto = new TrailerDto()
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

    public class SearchTrailerService : SearchModelService<Trailer>
    {
        public SearchTrailerService(IRepository repository, ILogger<SearchModelService<Trailer>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(string name)
        {
            IEnumerable<Trailer> trailers = await _repository.Get<Trailer>(t => $"{t.Model}{t.Number.Replace(" ", "")}".ToLower().Contains(name.Replace(" ","").ToLower()), null, "Carrier");
            List<TrailerDto> dtos = new List<TrailerDto>();

            foreach (var trailer in trailers)
            {
                TrailerDto dto = new TrailerDto()
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

    public class DeleteTrailerService : DeleteModelService<Trailer>
    {
        public DeleteTrailerService(IRepository repository) : base(repository)
        {
        }
    }

    public class AddTrailerService : AddModelService<TrailerDto>
    {
        public AddTrailerService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(TrailerDto dto)
        {
            Trailer trailer = new Trailer()
            {
                Id = Guid.NewGuid(),
                Model = dto.Model,
                Number = dto.Number,
            };

            if (dto.Carrier != null)
            {
                Carrier carrier = await _repository.GetById<Carrier>(dto.Carrier.Id);
                trailer.Carrier = carrier;
            }

            return await _repository.Update(trailer);
        }
    }

    public class UpdateTrailerService : UpdateModelService<TrailerDto>
    {
        public UpdateTrailerService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(TrailerDto dto)
        {
            Trailer trailer = await _repository.GetById<Trailer>(dto.Id);

            trailer.Model = dto.Model;
            trailer.Number = dto.Number;

            if (dto.Carrier != null)
            {
                Carrier carrier = await _repository.GetById<Carrier>(dto.Carrier.Id);
                trailer.Carrier = carrier;
            }
            else
            {
                trailer.Carrier = null;
                trailer.CarrierId = null;
            }

            IEnumerable<Driver> drivers = await _repository.Get<Driver>(d => d.TrailerId == dto.Id);

            foreach (Driver driver in drivers) 
            {
                if (driver.CarrierId != trailer.CarrierId) 
                {
                    driver.Trailer = null;
                    driver.TrailerId = null;

                    await _repository.Update(driver);
                }
            }

            return await _repository.Update(trailer);
        }
    }
}
