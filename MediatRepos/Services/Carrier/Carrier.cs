using DTOs;
using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq.Expressions;
using Utilities.Interfaces;

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

    public class GetIdCarrierService : IRequestHandler<GetId<CarrierDto>, IServiceResult<object>>
    {
        private IRepository _repository;
        public GetIdCarrierService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(GetId<CarrierDto> request, CancellationToken cancellationToken)
        {
            Carrier company = await _repository.GetById<Carrier>(request.Id);

            if (company == null)
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Перевозчик не найден"
                };
            }
            else 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = true,
                    Result = CarrierConverter.Convert(company)
                };
            }
        }
    }

    public class GetRangeCarrierService : IRequestHandler<GetRange<CarrierDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public GetRangeCarrierService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(GetRange<CarrierDto> request, CancellationToken cancellationToken)
        {
            IEnumerable<Carrier> carriers = await _repository.GetRange<Carrier>(request.Start, request.End, q => q.OrderBy(c => c.Name));

            List<CarrierDto> dtos = new List<CarrierDto>();

            foreach (var carrier in carriers)
            {
                dtos.Add(CarrierConverter.Convert(carrier));
            }

            return new MediatorServiceResult()
            {
                IsSuccess = true,
                Result = dtos,
            };
        }
    }

    public class GetFilterCarrierService : IRequestHandler<GetFilter<CarrierDto>, IServiceResult<object>>
    {
        protected IRepository _repository;
        protected ILogger<GetFilterCarrierService> _logger;

        public GetFilterCarrierService(IRepository repository, ILogger<GetFilterCarrierService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IServiceResult<object>> Handle(GetFilter<CarrierDto> request, CancellationToken cancellationToken)
        {
            Expression<Func<Carrier, bool>> filter = GetFilter(request.PropertyName, request.Params);

            IEnumerable<Carrier> carriers = await _repository.Get(filter);
            List<CarrierDto> dtos = new List<CarrierDto>();

            foreach (var carrier in carriers)
            {
                dtos.Add(CarrierConverter.Convert(carrier));
            }

            return new MediatorServiceResult()
            {
                IsSuccess = true,
                Result = dtos,
            };
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

    public class DeleteCarrierService : IRequestHandler<Delete<CarrierDto>, IServiceResult<object>>
    {
        private IRepository _repository;
        private IFileManager _fileManager;

        public DeleteCarrierService(IRepository repository, IFileManager fileManager)
        {
            _repository = repository;
            _fileManager = fileManager;
        }

        public async Task<IServiceResult<object>> Handle(Delete<CarrierDto> request, CancellationToken cancellationToken)
        {
            if (await _repository.Remove<Carrier>(request.Id))
            {
                IEnumerable<Models.Sub.File> files = await _repository.Get<Models.Sub.File>(f => f.EntityType == nameof(Carrier) && f.EntityId == request.Id);

                if (files.Any())
                {
                    string catalog = Path.GetFileName(Path.GetDirectoryName(files.First().FullFilePath));

                    await _fileManager.RemoveAllFiles(nameof(Carrier), catalog);
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
                    ErrorMessage = "Не удалось удалить перевозчика"
                };
            }
        }
    }

    public class AddCarrierService : IRequestHandler<Update<CarrierDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public AddCarrierService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(Update<CarrierDto> request, CancellationToken cancellationToken)
        {
            CarrierDto dto = request.Value;
            Guid id = await _repository.Add(CarrierConverter.Convert(dto));

            if (id == Guid.Empty)
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Не удалось добавить перевозчика"
                };
            }
            else 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = true,
                    Result = true
                };
            }
        }
    }

    public class UpdateCarrierService : IRequestHandler<Update<CarrierDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public UpdateCarrierService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(Update<CarrierDto> request, CancellationToken cancellationToken)
        {
            CarrierDto dto = request.Value;

            Carrier company = await _repository.GetById<Carrier>(dto.Id);

            if (company == null)
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Перевозчик не найден"
                };
            }

            company.Name = dto.Name;
            company.Vat = (short)dto.Vat;
            company.Address = dto.Address;
            company.InnKpp = dto.InnKpp;
            company.Phones = string.Join(";", dto.Phones);
            company.Emails = string.Join(";", dto.Emails);

            if (await _repository.Update(company))
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
                    ErrorMessage = "Не удалось обновить перевозчика"
                };
            }
        }
    }
}
