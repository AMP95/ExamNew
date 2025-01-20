﻿using DTOs;
using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq.Expressions;

namespace MediatorServices
{
    public class GetIdPaymentService : GetIdModelService<PaymentDto>
    {
        public GetIdPaymentService(IRepository repository, ILogger<GetIdModelService<PaymentDto>> logger) : base(repository, logger)
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

    public class GetFilterPaymentService : IRequestHandler<GetFilter<PaymentDto>, object>
    {
        protected IRepository _repository;
        protected ILogger<GetFilterPaymentService> _logger;

        public GetFilterPaymentService(IRepository repository, ILogger<GetFilterPaymentService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<object> Handle(GetFilter<PaymentDto> request, CancellationToken cancellationToken)
        {
            Expression<Func<Payment, bool>> filter = GetFilter(request.PropertyName, request.Params);
            return await Get(filter);
        }

        protected async Task<object> Get(Expression<Func<Payment, bool>> filter)
        {
            IEnumerable<Payment> documents = await _repository.Get<Payment>(filter);
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

        protected Expression<Func<Payment, bool>> GetFilter(string property, params object[] parameters)
        {
            Expression<Func<Payment, bool>> filter = null;
            try
            {

                switch (property)
                {
                    case nameof(PaymentDto.ContractId):
                        Guid guid = (Guid)parameters[0];
                        filter = d => d.ContractId == guid;
                        break;
                    case nameof(PaymentDto.Direction):
                        DocumentDirection direction = (DocumentDirection)parameters[0];
                        filter = d => d.DocumentDirection == (short)direction;
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

    public class DeletePaymentService : IRequestHandler<Delete<PaymentDto>, bool>
    {
        private IRepository _repository;

        public DeletePaymentService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(Delete<PaymentDto> request, CancellationToken cancellationToken)
        {
            return await _repository.Remove<Payment>(request.Id);
        }
    }

    public class AddPaymentService : AddModelService<PaymentDto>
    {
        public AddPaymentService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<Guid> Add(PaymentDto dto)
        {
            Payment document = new Payment()
            {
                CreationDate = dto.CreationDate,
                ContractId = dto.ContractId,
                Summ = dto.Summ,
                DocumentDirection = (short)dto.Direction,
                Number = dto.Number,
            };

            return await _repository.Add(document);
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
