﻿using DTOs;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq.Expressions;

namespace MediatorServices
{
    public static class ClientConverter
    {
        public static ClientDto Convert(Client client) 
        {
            return new ClientDto()
            {
                Id = client.Id,
                Name = client.Name,
                Address = client.Address,
                Emails = client.Emails.Split(';').ToList(),
                Phones = client.Phones.Split(';').ToList(),
                InnKpp = client.InnKpp,
                IsPriority = client.IsPriority,
            };
        }

        public static Client Convert(ClientDto dto)
        {
            return new Client()
            {
                Name = dto.Name,
                Address = dto.Address,
                InnKpp = dto.InnKpp,
                Phones = string.Join(";", dto.Phones),
                Emails = string.Join(";", dto.Emails),
                IsPriority = dto.IsPriority,
            };
        }
    }

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
                dto = ClientConverter.Convert(company);
            }
            return dto;
        }
    }

    public class GetRangeClientService : GetRangeModelService<Client>
    {
        public GetRangeClientService(IRepository repository, ILogger<GetRangeModelService<Client>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(int start, int end)
        {
            IEnumerable<Client> clients = await _repository.GetRange<Client>(start, end, q => q.OrderBy(c => c.Name));
            List<ClientDto> dtos = new List<ClientDto>();

            foreach (var client in clients)
            {
                dtos.Add(ClientConverter.Convert(client));
            }

            return dtos;
        }
    }

    public class GetFilterClientService : GetFilterModelService<Client>
    {
        public GetFilterClientService(IRepository repository, ILogger<GetFilterModelService<Client>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Expression<Func<Client, bool>> filter)
        {
            IEnumerable<Client> carriers = await _repository.Get<Client>(filter);
            List<ClientDto> dtos = new List<ClientDto>();

            foreach (var carrier in carriers)
            {
                dtos.Add(ClientConverter.Convert(carrier));
            }

            return dtos;
        }

        protected override Expression<Func<Client, bool>> GetFilter(string property, params object[] parameters)
        {
            Expression<Func<Client, bool>> filter = null;

            try
            {
                switch (property)
                {
                    case nameof(ClientDto.Name):
                        string name = (string)parameters[0];
                        filter = c => c.Name.ToLower().Contains(name.ToLower());
                        break;
                    case nameof(ClientDto.InnKpp):
                        string innKpp = (string)parameters[0];
                        filter = c => c.InnKpp.ToLower().Contains(innKpp.ToLower());
                        break;
                    case nameof(ClientDto.IsPriority):
                        bool isPriority = (bool)parameters[0];
                        filter = c => c.IsPriority == isPriority;
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
            return await _repository.Update(ClientConverter.Convert(dto));
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
            company.IsPriority = dto.IsPriority;

            return await _repository.Update(company);
        }
    }
}
