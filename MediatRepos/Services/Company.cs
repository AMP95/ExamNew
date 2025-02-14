using DTOs;
using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq.Expressions;
using Utilities.Interfaces;

namespace MediatorServices
{
    public static class ClientConverter
    {
        public static CompanyDto Convert(Company client) 
        {
            return new CompanyDto()
            {
                Id = client.Id,
                Name = client.Name,
                Address = client.Address,
                Emails = client.Emails.Split(';').ToList(),
                Phones = client.Phones.Split(';').ToList(),
                InnKpp = client.InnKpp,
                Type = (CompanyType)client.Type,
            };
        }

        public static Company Convert(CompanyDto dto)
        {
            return new Company()
            {
                Name = dto.Name,
                Address = dto.Address,
                InnKpp = dto.InnKpp,
                Phones = string.Join(";", dto.Phones),
                Emails = string.Join(";", dto.Emails),
                Type = (short)dto.Type,
            };
        }
    }

    public class GetIdClientService : IRequestHandler<GetId<CompanyDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public GetIdClientService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(GetId<CompanyDto> request, CancellationToken cancellationToken)
        {
            Company company = await _repository.GetById<Company>(request.Id);

            if (company == null)
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Организация не найдена"
                };
            }
            else 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = true,
                    Result = ClientConverter.Convert(company)
                };
            }
        }
    }

    public class GetRangeClientService : IRequestHandler<GetRange<CompanyDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public GetRangeClientService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(GetRange<CompanyDto> request, CancellationToken cancellationToken)
        {
            IEnumerable<Company> clients = await _repository.GetRange<Company>(request.Start, request.End, q => q.OrderBy(c => c.Name));

            List<CompanyDto> dtos = new List<CompanyDto>();

            foreach (var client in clients)
            {
                if (client.Type == (short)CompanyType.Client)
                {
                    dtos.Add(ClientConverter.Convert(client));
                }
            }

            return new MediatorServiceResult()
            {
                IsSuccess = true,
                Result = dtos
            };
        }
    }

    public class GetFilterClientService : IRequestHandler<GetFilter<CompanyDto>, IServiceResult<object>>
    {
        protected IRepository _repository;
        protected ILogger<GetFilterClientService> _logger;

        public GetFilterClientService(IRepository repository, ILogger<GetFilterClientService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IServiceResult<object>> Handle(GetFilter<CompanyDto> request, CancellationToken cancellationToken)
        {
            Expression<Func<Company, bool>> filter = GetFilter(request.PropertyName, request.Params);

            IEnumerable<Company> carriers = await _repository.Get<Company>(filter);
            List<CompanyDto> dtos = new List<CompanyDto>();

            foreach (var carrier in carriers)
            {
                dtos.Add(ClientConverter.Convert(carrier));
            }

            return new MediatorServiceResult()
            {
                IsSuccess = true,
                Result = dtos
            };
        }

        protected Expression<Func<Company, bool>> GetFilter(string property, params object[] parameters)
        {
            Expression<Func<Company, bool>> filter = null;

            try
            {
                switch (property)
                {
                    case nameof(CompanyDto.Name):
                        string name = (string)parameters[0];
                        filter = c => c.Name.ToLower().Contains(name.ToLower());
                        break;
                    case nameof(CompanyDto.InnKpp):
                        string innKpp = (string)parameters[0];
                        filter = c => c.InnKpp.ToLower().Contains(innKpp.ToLower());
                        break;
                    case nameof(CompanyDto.Type):
                        if (Enum.TryParse<CompanyType>((string)parameters[0], out CompanyType type)) 
                        {
                            filter = c => c.Type == (short)type;
                        }
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


    public class DeleteClientService : IRequestHandler<Delete<CompanyDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public DeleteClientService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(Delete<CompanyDto> request, CancellationToken cancellationToken)
        {
            if (await _repository.Remove<Company>(request.Id))
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
                    ErrorMessage = "Не удалось удалить компанию",
                    Result = false
                };
            }
        }
    }

    public class AddClientService : IRequestHandler<Add<CompanyDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public AddClientService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(Add<CompanyDto> request, CancellationToken cancellationToken)
        {
            Guid id = await _repository.Add(ClientConverter.Convert(request.Value));

            if (id == Guid.Empty)
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Не удалось добавить компанию",
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

    public class UpdateClientService : IRequestHandler<Update<CompanyDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public UpdateClientService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(Update<CompanyDto> request, CancellationToken cancellationToken)
        {
            CompanyDto dto = request.Value;

            Company company = await _repository.GetById<Company>(dto.Id);

            if (dto == null) 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Не удалось найти компанию",
                    Result  = false
                };
            }

            company.Name = dto.Name;
            company.Address = dto.Address;
            company.InnKpp = dto.InnKpp;
            company.Phones = string.Join(";", dto.Phones);
            company.Emails = string.Join(";", dto.Emails);
            company.Type = (short)dto.Type;

            if (await _repository.Update(company))
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
                    ErrorMessage = "Не удалось обновить компанию",
                    Result = false
                };
            }
        }
    }
}
