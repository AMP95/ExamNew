using DTOs;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;

namespace MediatorServices
{
    public class GetIdCompanyService : GetIdModelService<Company>
    {
        public GetIdCompanyService(IRepository repository, ILogger<GetIdModelService<Company>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            Company company = await _repository.GetById<Company>(id);
            CompanyDto dto = null;

            if (company != null)
            {
                dto = new CompanyDto()
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

    public class SearchCompanyService : SearchModelService<Company>
    {
        public SearchCompanyService(IRepository repository, ILogger<SearchModelService<Company>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(string name)
        {
            IEnumerable<Company> carriers = await _repository.Get<Company>(c => c.Name.ToLower().Contains(name.ToLower()));
            List<CompanyDto> dtos = new List<CompanyDto>();

            foreach (var carrier in carriers)
            {
                dtos.Add(new CompanyDto()
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

    public class GetRangeCompanyService : GetRangeModelService<Company>
    {
        public GetRangeCompanyService(IRepository repository, ILogger<GetRangeModelService<Company>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(int start, int end)
        {
            IEnumerable<Company> carriers = await _repository.GetRange<Company>(start, end, q => q.OrderBy(c => c.Name));
            List<CompanyDto> dtos = new List<CompanyDto>();

            foreach (var carrier in carriers)
            {
                dtos.Add(new CompanyDto()
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

    public class DeleteCompanyService : DeleteModelService<Company>
    {
        public DeleteCompanyService(IRepository repository) : base(repository)
        {
        }
    }

    public class AddCompanyService : AddModelService<CompanyDto>
    {
        public AddCompanyService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(CompanyDto dto)
        {
            Company company = new Company()
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

    public class UpdateCompanyService : UpdateModelService<CompanyDto>
    {
        public UpdateCompanyService(IRepository repository) : base(repository)
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

            return await _repository.Update(company);
        }
    }
}
