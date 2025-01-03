using DTOs;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq.Expressions;

namespace MediatorServices
{
    public class GetIdCarrierService : GetIdModelService<Carrier>
    {
        public GetIdCarrierService(IRepository repository, ILogger<GetIdModelService<Carrier>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            IEnumerable<Carrier> companies = await _repository.Get<Carrier>(c => c.Id == id, null, "Drivers,Trucks,Trailers");
            CarrierDto dto = null;

            if (companies.Any())
            {
                Carrier company = companies.First();

                dto = new CarrierDto()
                {
                    Id = company.Id,
                    Vat = (VAT)company.Vat,
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

    public class GetFilteredCarrierService : GetFilteredModelService<Carrier>
    {
        public GetFilteredCarrierService(IRepository repository, ILogger<GetIdModelService<Carrier>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Expression<Func<Carrier, bool>> filter)
        {
            IEnumerable<Carrier> companys = await _repository.Get(filter);
            List<CarrierDto> dtos = new List<CarrierDto>();

            foreach (var company in companys)
            {
                dtos.Add(new CarrierDto()
                {
                    Id = company.Id,
                    Vat = (VAT)company.Vat,
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

    public class DeleteCarrierService : DeleteModelService<Carrier>
    {
        public DeleteCarrierService(IRepository repository) : base(repository)
        {
        }
    }

    public class AddCarrierService : AddModelService<CarrierDto>
    {
        public AddCarrierService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(CarrierDto dto)
        {
            Carrier company = new Carrier()
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Vat = (short)dto.Vat, 
                Address = dto.Address,
                InnKpp = dto.InnKpp,
                Phones = string.Join(";", dto.Phones),
                Emails = string.Join(";", dto.Emails)
            };

            return await _repository.Update(company);
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

            company.Name = dto.Name;
            company.Vat = company.Vat;
            company.Address = dto.Address;
            company.InnKpp = dto.InnKpp;
            company.Phones = string.Join(";", dto.Phones);
            company.Emails = string.Join(";", dto.Emails);

            return await _repository.Update(company);
        }
    }
}
