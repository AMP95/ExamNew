using DTOs;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;

namespace MediatorServices
{
    public class GetIdPaymentService : GetIdModelService<Payment>
    {
        public GetIdPaymentService(IRepository repository, ILogger<GetIdModelService<Payment>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            Payment document = await _repository.GetById<Payment>(id);
            PaymentDto dto = null;

            if (document != null)
            {
                dto = new PaymentDto()
                {
                    Id = document.Id,
                    ContractId = document.ContractId,
                    CreationDate = document.CreationDate,
                    Direction = (DocumentDirection)document.DocumentDirection,
                    Number = document.Number,
                    Summ = document.Summ
                };
            }
            return dto;
        }
    }

    public class GetMainIdPaymentService : GetMainIdModelService<Payment>
    {
        public GetMainIdPaymentService(IRepository repository, ILogger<GetMainIdModelService<Payment>> logger) : base(repository, logger)
        {
        }

        protected async override Task<object> Get(Guid id)
        {
            IEnumerable<Payment> documents = await _repository.Get<Payment>(d => d.ContractId == id);
            List<PaymentDto> dtos = new List<PaymentDto>();

            foreach (var document in documents)
            {
                dtos.Add(new PaymentDto()
                {
                    Id = document.Id,
                    ContractId = document.ContractId,
                    CreationDate = document.CreationDate,
                    Direction = (DocumentDirection)document.DocumentDirection,
                    Number = document.Number,
                    Summ = document.Summ
                });
            }

            return dtos;
        }
    }

    public class DeletePaymentService : DeleteModelService<Payment>
    {
        public DeletePaymentService(IRepository repository) : base(repository)
        {
        }
    }

    public class AddPaymentService : AddModelService<PaymentDto>
    {
        public AddPaymentService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(PaymentDto dto)
        {
            Payment document = new Payment()
            {
                CreationDate = dto.CreationDate,
                ContractId = dto.ContractId,
                Summ = dto.Summ,
                DocumentDirection = (short)dto.Direction,
                Number = dto.Number,
            };

            return await _repository.Update(document);
        }
    }

    public class UpdatePaymentService : UpdateModelService<PaymentDto>
    {
        public UpdatePaymentService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(PaymentDto dto)
        {
            Payment document = await _repository.GetById<Payment>(dto.Id);

            document.Id = dto.Id;
            document.CreationDate = dto.CreationDate;
            document.DocumentDirection = (short)dto.Direction;
            document.Number = dto.Number;
            document.Summ = dto.Summ;

            return await _repository.Update(document);
        }
    }
}
