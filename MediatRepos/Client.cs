using DTOs;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;

namespace MediatorServices
{
    public class GetIdClientService : GetIdModelService<Client>
    {
        public GetIdClientService(IRepository repository, ILogger<GetIdModelService<Client>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            Client company = await _repository.GetById<Client>(id);
            ClientDto dto = null;

            if (company != null)
            {
                dto = new ClientDto()
                {
                    Id = company.Id,
                    Name = company.Name,
                    Address = company.Address,
                    Emails = company.Emails.Split(';').ToList(),
                    Phones = company.Phones.Split(';').ToList(),
                    InnKpp = company.InnKpp
                };
            }
            return dto;
        }
    }

    public class SearchClientService : SearchModelService<Client>
    {
        public SearchClientService(IRepository repository, ILogger<SearchModelService<Client>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(string name)
        {
            IEnumerable<Client> carriers = await _repository.Get<Client>(c => c.Name.ToLower().Contains(name.ToLower()));
            List<ClientDto> dtos = new List<ClientDto>();

            foreach (var carrier in carriers)
            {
                dtos.Add(new ClientDto()
                {
                    Id = carrier.Id,
                    Name = carrier.Name,
                    Address = carrier.Address,
                    Emails = carrier.Emails.Split(';').ToList(),
                    Phones = carrier.Phones.Split(';').ToList(),
                    InnKpp = carrier.InnKpp
                });
            }

            return dtos;
        }
    }

    public class GetRangeClientService : GetRangeModelService<Client>
    {
        public GetRangeClientService(IRepository repository, ILogger<GetRangeModelService<Client>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(int start, int end)
        {
            IEnumerable<Client> carriers = await _repository.GetRange<Client>(start, end, q => q.OrderBy(c => c.Name));
            List<ClientDto> dtos = new List<ClientDto>();

            foreach (var carrier in carriers)
            {
                dtos.Add(new ClientDto()
                {
                    Id = carrier.Id,
                    Name = carrier.Name,
                    Address = carrier.Address,
                    Emails = carrier.Emails.Split(';').ToList(),
                    Phones = carrier.Phones.Split(';').ToList(),
                    InnKpp = carrier.InnKpp
                });
            }

            return dtos;
        }
    }

    public class DeleteClientService : DeleteModelService<Client>
    {
        public DeleteClientService(IRepository repository) : base(repository)
        {
        }
    }

    public class AddClientService : AddModelService<ClientDto>
    {
        public AddClientService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(ClientDto dto)
        {
            Client company = new Client()
            {
                Name = dto.Name,
                Address = dto.Address,
                InnKpp = dto.InnKpp,
                Phones = string.Join(";", dto.Phones),
                Emails = string.Join(";", dto.Emails)
            };

            return await _repository.Update(company);
        }
    }

    public class UpdateClientService : UpdateModelService<ClientDto>
    {
        public UpdateClientService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(ClientDto dto)
        {
            Client company = await _repository.GetById<Client>(dto.Id);

            company.Name = dto.Name;
            company.Address = dto.Address;
            company.InnKpp = dto.InnKpp;
            company.Phones = string.Join(";", dto.Phones);
            company.Emails = string.Join(";", dto.Emails);

            return await _repository.Update(company);
        }
    }
}
