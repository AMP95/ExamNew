using DTOs;
using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq.Expressions;

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

    public class GetIdClientService : GetIdModelService<CompanyDto>
    {
        public GetIdClientService(IRepository repository, ILogger<GetIdModelService<CompanyDto>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            Company company = await _repository.GetById<Company>(id);
            CompanyDto dto = null;

            if (company != null)
            {
                dto = ClientConverter.Convert(company);
            }
            return dto;
        }
    }

    public class GetRangeClientService : GetRangeModelService<CompanyDto>
    {
        public GetRangeClientService(IRepository repository, ILogger<GetRangeModelService<CompanyDto>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(int start, int end)
        {
            IEnumerable<Company> clients = await _repository.GetRange<Company>(start, end, q => q.OrderBy(c => c.Name));
            List<CompanyDto> dtos = new List<CompanyDto>();

            foreach (var client in clients)
            {
                if (client.Type == (short)CompanyType.Client)
                {
                    dtos.Add(ClientConverter.Convert(client));
                }
            }

            return dtos;
        }
    }

    public class GetFilterClientService : IRequestHandler<GetFilter<CompanyDto>, object>
    {
        protected IRepository _repository;
        protected ILogger<GetFilterClientService> _logger;

        public GetFilterClientService(IRepository repository, ILogger<GetFilterClientService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<object> Handle(GetFilter<CompanyDto> request, CancellationToken cancellationToken)
        {
            Expression<Func<Company, bool>> filter = GetFilter(request.PropertyName, request.Params);
            return await Get(filter);
        }

        protected async Task<object> Get(Expression<Func<Company, bool>> filter)
        {
            IEnumerable<Company> carriers = await _repository.Get<Company>(filter);
            List<CompanyDto> dtos = new List<CompanyDto>();

            foreach (var carrier in carriers)
            {
                dtos.Add(ClientConverter.Convert(carrier));
            }

            return dtos;
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


    public class DeleteClientService : IRequestHandler<Delete<CompanyDto>, bool>
    {
        private IRepository _repository;

        public DeleteClientService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(Delete<CompanyDto> request, CancellationToken cancellationToken)
        {
            return await _repository.Remove<Company>(request.Id);
        }
    }

    public class AddClientService : AddModelService<CompanyDto>
    {
        public AddClientService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<Guid> Add(CompanyDto dto)
        {
            return await _repository.Add(ClientConverter.Convert(dto));
        }
    }

    public class UpdateClientService : UpdateModelService<CompanyDto>
    {
        public UpdateClientService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(CompanyDto dto)
        {
            Company company = await _repository.GetById<Company>(dto.Id);

            company.Name = dto.Name;
            company.Address = dto.Address;
            company.InnKpp = dto.InnKpp;
            company.Phones = string.Join(";", dto.Phones);
            company.Emails = string.Join(";", dto.Emails);
            company.Type = (short)dto.Type;

            return await _repository.Update(company);
        }
    }
}
