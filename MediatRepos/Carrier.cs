using DTOs;
using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq.Expressions;

namespace MediatorServices
{
    public static class CarrierConverter
    {
        public static CarrierDto Convert(Carrier client)
        {
            return new CarrierDto()
            {
                Id = client.Id,
                Name = client.Name,
                Address = client.Address,
                Emails = client.Emails.Split(';').ToList(),
                Phones = client.Phones.Split(';').ToList(),
                InnKpp = client.InnKpp,
                Vat = (VAT)client.Vat,
            };
        }

        public static Carrier Convert(CarrierDto dto)
        {
            return new Carrier()
            {
                Name = dto.Name,
                Address = dto.Address,
                InnKpp = dto.InnKpp,
                Phones = string.Join(";", dto.Phones),
                Emails = string.Join(";", dto.Emails),
                Vat = (short)dto.Vat,
            };
        }
    }

    public class GetIdCarrierService : GetIdModelService<CarrierDto>
    {
        public GetIdCarrierService(IRepository repository, ILogger<GetIdModelService<CarrierDto>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            Carrier company = await _repository.GetById<Carrier>(id);
            CarrierDto dto = null;

            if (company != null)
            {
                dto = CarrierConverter.Convert(company);
            }
            return dto;
        }
    }

    public class GetRangeCarrierService : GetRangeModelService<CarrierDto>
    {
        public GetRangeCarrierService(IRepository repository, ILogger<GetRangeModelService<CarrierDto>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(int start, int end)
        {
            IEnumerable<Carrier> carriers = await _repository.GetRange<Carrier>(start, end, q => q.OrderBy(c => c.Name));
            List<CarrierDto> dtos = new List<CarrierDto>();
            foreach (var carrier in carriers) 
            {
                dtos.Add(CarrierConverter.Convert(carrier));
            }

            return dtos;
        }
    }

    public class GetFilterCarrierService : IRequestHandler<GetFilter<CarrierDto>, object>
    {
        protected IRepository _repository;
        protected ILogger<GetFilterCarrierService> _logger;

        public GetFilterCarrierService(IRepository repository, ILogger<GetFilterCarrierService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<object> Handle(GetFilter<CarrierDto> request, CancellationToken cancellationToken)
        {
            Expression<Func<Carrier, bool>> filter = GetFilter(request.PropertyName, request.Params);
            return await Get(filter);
        }

        protected async Task<object> Get(Expression<Func<Carrier, bool>> filter)
        {
            IEnumerable<Carrier> carriers = await _repository.Get<Carrier>(filter);
            List<CarrierDto> dtos = new List<CarrierDto>();

            foreach (var carrier in carriers)
            {
                dtos.Add(CarrierConverter.Convert(carrier));
            }

            return dtos;
        }

        protected Expression<Func<Carrier, bool>> GetFilter(string property, params object[] parameters)
        {
            Expression<Func<Carrier, bool>> filter = null;

            try
            {
                switch (property)
                {
                    case nameof(CarrierDto.Name):
                        string name = (string)parameters[0];
                        filter = c => c.Name.ToLower().Contains(name.ToLower());
                        break;
                    case nameof(CarrierDto.InnKpp):
                        string innKpp = (string)parameters[0];
                        filter = c => c.InnKpp.ToLower().Contains(innKpp.ToLower());
                        break;
                    case nameof(CarrierDto.Vat):
                        VAT vat = (VAT)Enum.Parse(typeof(VAT), parameters[0].ToString());
                        filter = c => c.Vat == (short)vat;
                        break;
                }
            }
            catch (Exception ex) 
            {
                filter = c => false;
                _logger.LogError(ex, ex.Message);
            }

            return filter;
        }
    }

    public class DeleteCarrierService : IRequestHandler<Delete<CarrierDto>, bool>
    {
        private IRepository _repository;

        public DeleteCarrierService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(Delete<CarrierDto> request, CancellationToken cancellationToken)
        {
            return await _repository.Remove<Carrier>(request.Id);
        }
    }

    public class AddCarrierService : AddModelService<CarrierDto>
    {
        public AddCarrierService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<Guid> Add(CarrierDto dto)
        {
            return await _repository.Add(CarrierConverter.Convert(dto));
        }
    }

    public class UpdateCarrierService : UpdateModelService<CarrierDto>
    {
        public UpdateCarrierService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(CarrierDto dto)
        {
            Carrier company = await _repository.GetById<Carrier>(dto.Id);

            if (company != null)
            {
                company.Name = dto.Name;
                company.Vat = (short)dto.Vat;
                company.Address = dto.Address;
                company.InnKpp = dto.InnKpp;
                company.Phones = string.Join(";", dto.Phones);
                company.Emails = string.Join(";", dto.Emails);

                return await _repository.Update(company);
            }
            return false;
        }
    }
}
