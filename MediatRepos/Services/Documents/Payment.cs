using DTOs;
using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq.Expressions;
using Utilities.Interfaces;

namespace MediatorServices
{
    public class GetIdPaymentService : IRequestHandler<GetId<PaymentDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public GetIdPaymentService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(GetId<PaymentDto> request, CancellationToken cancellationToken)
        {
            Payment payment = await _repository.GetById<Payment>(request.Id);

            if (payment == null)
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Оплата не найдена"
                };
            }
            else
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = true,
                    Result = new PaymentDto()
                    {
                        Id = payment.Id,
                        ContractId = payment.ContractId,
                        CreationDate = payment.CreationDate,
                        Direction = (DocumentDirection)payment.DocumentDirection,
                        Number = payment.Number,
                        Summ = payment.Summ
                    }
                };
            }
        }
    }

    public class GetFilterPaymentService : IRequestHandler<GetFilter<PaymentDto>, IServiceResult<object>>
    {
        private IRepository _repository;
        private ILogger<GetFilterPaymentService> _logger;

        public GetFilterPaymentService(IRepository repository, ILogger<GetFilterPaymentService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IServiceResult<object>> Handle(GetFilter<PaymentDto> request, CancellationToken cancellationToken)
        {
            Expression<Func<Payment, bool>> filter = GetFilter(request.PropertyName, request.Params);

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

            return new MediatorServiceResult()
            {
                IsSuccess = true,
                Result = dtos
            };
        }

        protected Expression<Func<Payment, bool>> GetFilter(string property, params object[] parameters)
        {
            Expression<Func<Payment, bool>> filter = null;
            try
            {
                switch (property)
                {
                    case nameof(PaymentDto.ContractId):
                        if (Guid.TryParse(parameters[0].ToString(), out Guid id))
                        {
                            filter = d => d.ContractId == id;
                        }
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

    public class DeletePaymentService : IRequestHandler<Delete<PaymentDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public DeletePaymentService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(Delete<PaymentDto> request, CancellationToken cancellationToken)
        {
            Payment payment = await _repository.GetById<Payment>(request.Id);

            if (payment == null) 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Оплата не найдена"
                };
            }

            if (await _repository.Remove<Payment>(payment.Id))
            {
                await StatusUpdater.UpdateContractStatus(payment.ContractId, _repository);

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
                    ErrorMessage = "Не удалось удалить оплату"
                };
            } 
        }
    }

    public class AddPaymentService : IRequestHandler<Add<PaymentDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public AddPaymentService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(Add<PaymentDto> request, CancellationToken cancellationToken)
        {
            PaymentDto dto = request.Value;

            Payment payment = new Payment()
            {
                CreationDate = dto.CreationDate,
                ContractId = dto.ContractId,
                Summ = dto.Summ,
                DocumentDirection = (short)dto.Direction,
                Number = dto.Number,
            };

            Guid payId = await _repository.Add(payment);

            if (payId == Guid.Empty)
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    Result = "Не удалось добавить оплату",
                };
            }
            else
            {
                await StatusUpdater.UpdateContractStatus(dto.ContractId, _repository);

                return new MediatorServiceResult()
                {
                    IsSuccess = true,
                    Result = true,
                };
            }
        }
    }

    public class UpdatePaymentService : IRequestHandler<Update<PaymentDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public UpdatePaymentService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(Update<PaymentDto> request, CancellationToken cancellationToken)
        {
            PaymentDto dto = request.Value;

            Payment payment = await _repository.GetById<Payment>(dto.Id);

            if (payment == null)
            {
                return new MediatorServiceResult()
                {
                    Result = false,
                    ErrorMessage = "Облата не найдена"
                };
            }

            payment.Id = dto.Id;
            payment.CreationDate = dto.CreationDate;
            payment.DocumentDirection = (short)dto.Direction;
            payment.Number = dto.Number;
            payment.Summ = dto.Summ;

            if (await _repository.Update(payment))
            {
                await StatusUpdater.UpdateContractStatus(dto.ContractId, _repository);

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
                    ErrorMessage = "Не удалось обновить оплату"
                };
            }
        }
    }
}
