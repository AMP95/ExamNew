using DTOs;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq.Expressions;

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

    public class GetFilteredCompanyService : GetFilteredModelService<Company>
    {
        public GetFilteredCompanyService(IRepository repository, ILogger<GetIdModelService<Company>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Expression<Func<Company, bool>> filter)
        {
            IEnumerable<Company> companys = await _repository.Get(filter);
            List<CompanyDto> dtos = new List<CompanyDto>();

            foreach (var company in companys)
            {
                dtos.Add(new CompanyDto()
                {
                    Id = company.Id,
                    Name = company.Name,
                    Address = company.Address,
                    Emails = company.Emails.Split(';').ToList(),
                    Phones = company.Phones.Split(';').ToList(),
                    InnKpp = company.InnKpp
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
                Id = Guid.NewGuid(),
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
