using DTOs;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;

namespace MediatorServices
{
    public class GetIdCarrierService : GetIdModelService<Carrier>
    {
        public GetIdCarrierService(IRepository repository, ILogger<GetIdModelService<Carrier>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            Carrier company = await _repository.GetById<Carrier>(id);
            CarrierDto dto = null;

            if (company != null)
            {
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

    public class SearchCarrierService : SearchModelService<Carrier>
    {
        public SearchCarrierService(IRepository repository, ILogger<SearchModelService<Carrier>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(string name)
        {
            IEnumerable<Carrier> carriers = await _repository.Get<Carrier>(c => c.Name.ToLower().Contains(name.ToLower()) ,null, null);
            List<CarrierDto> dtos = new List<CarrierDto>();

            foreach (var carrier in carriers)
            {
                dtos.Add(new CarrierDto()
                {
                    Id = carrier.Id,
                    Vat = (VAT)carrier.Vat,
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

    public class GetRangeCarrierService : GetRangeModelService<Carrier>
    {
        public GetRangeCarrierService(IRepository repository, ILogger<GetRangeModelService<Carrier>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(int start, int end)
        {
            IEnumerable<Carrier> carriers = await _repository.GetRange<Carrier>(start, end, null);
            List<CarrierDto> dtos = new List<CarrierDto>();

            foreach (var carrier in carriers) 
            {
                dtos.Add(new CarrierDto() 
                {
                    Id = carrier.Id,
                    Vat = (VAT)carrier.Vat,
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

            if (company != null)
            {
                company.Name = dto.Name;
                company.Vat = company.Vat;
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
