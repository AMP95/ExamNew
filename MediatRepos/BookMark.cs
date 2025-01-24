using DTOs.Dtos;
using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.Sub;
using System.Linq.Expressions;

namespace MediatorServices
{
    internal static class BookMarksList
    {
        public static List<BookMarkDto> List = new List<BookMarkDto>()
        {
            new BookMarkDto(){ Name = "Номер", InsertView = "{{ContractNumber}}" },
            new BookMarkDto(){ Name = "Дата", InsertView = "{{ConractDate}}" },
            new BookMarkDto(){ Name = "ПеревозчикТоп", InsertView = "{{CarrierTop}}" },
            new BookMarkDto(){ Name = "Маршрут", InsertView = "{{Route}}" },
            new BookMarkDto(){ Name = "Вес", InsertView = "{{Weight}}" },
            new BookMarkDto(){ Name = "Объем", InsertView = "{{Volume}}" },
            new BookMarkDto(){ Name = "Грузоотправитель", InsertView = "{{CargoSender}}" },
            new BookMarkDto(){ Name = "Адрес погрузки", InsertView = "{{LoadingPoint}}" },
            new BookMarkDto(){ Name = "Дата погрузки", InsertView = "{{LoadingDate}}" },
            new BookMarkDto(){ Name = "Контакт погрузки", InsertView = "{{SenderPhone}}" },
            new BookMarkDto(){ Name = "Способ погрузки", InsertView = "{{LoadingType}}" },
            new BookMarkDto(){ Name = "Грузополучатель", InsertView = "{{CargoReciever}}" },
            new BookMarkDto(){ Name = "Адрес выгрузки", InsertView = "{{UnloadingPoint}}" },
            new BookMarkDto(){ Name = "Дата выгрузки", InsertView = "{{UnloadingDate}}" },
            new BookMarkDto(){ Name = "Контакт выгрузки", InsertView = "{{RecievePhone}}" },
            new BookMarkDto(){ Name = "Способ выгрузки", InsertView = "{{UnloadType}}" },
            new BookMarkDto(){ Name = "Стоимость услуг", InsertView = "{{Payment}}" },
            new BookMarkDto(){ Name = "Условия оплаты", InsertView = "{{PaymentCondition}}" },
            new BookMarkDto(){ Name = "Приоритет оплаты", InsertView = "{{PayPriority}}" },
            new BookMarkDto(){ Name = "Имя водиетля", InsertView = "{{Driver}}" },
            new BookMarkDto(){ Name = "Паспорт водителя", InsertView = "{{DriverPassport}}" },
            new BookMarkDto(){ Name = "Водитель контакт", InsertView = "{{DriverPone}}" },
            new BookMarkDto(){ Name = "ТС", InsertView = "{{Vehicle}}" },
            new BookMarkDto(){ Name = "ПеревозчикНиз", InsertView = "{{CarrierBottom}}" },
            new BookMarkDto(){ Name = "Перевозчик адрес", InsertView = "{{CarrierAddress}}" },
            new BookMarkDto(){ Name = "Перевозчик ИНН", InsertView = "{{CarrierInnKpp}}" },
            new BookMarkDto(){ Name = "Перевозчик контакт", InsertView = "{{CarrierPhone}}" },
            new BookMarkDto(){ Name = "Перевозчик почта", InsertView = "{{CarrierMail}}" },
        };
    }

    public class GetFilterBookMarkService : IRequestHandler<GetFilter<BookMarkDto>, object>
    {
        protected IRepository _repository;
        protected ILogger<GetFilterBookMarkService> _logger;

        public GetFilterBookMarkService(IRepository repository, 
                                        ILogger<GetFilterBookMarkService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<object> Handle(GetFilter<BookMarkDto> request, CancellationToken cancellationToken)
        {
            Expression<Func<BookMark, bool>> filter = GetFilter(request.PropertyName, request.Params);
            return await Get(filter);
        }

        protected async Task<object> Get(Expression<Func<BookMark, bool>> filter)
        {
            IEnumerable<BookMark> bookMarks = await _repository.Get<BookMark>(filter);

            if (!bookMarks.Any()) 
            {
                var list = BookMarksList.List;

                foreach (var bookMark in list) 
                {
                    await _repository.Add(new BookMark() { Name = bookMark.Name, InsertView = bookMark.InsertView });
                }

                bookMarks = await _repository.Get<BookMark>(filter);
            }

            List<BookMarkDto> dtos = new List<BookMarkDto>();

            foreach (var mark in bookMarks)
            {
                dtos.Add(new BookMarkDto() 
                {
                    Id = mark.Id,
                    Name = mark.Name,
                    InsertView = mark.InsertView,
                });
            }

            return dtos;
        }

        protected Expression<Func<BookMark, bool>> GetFilter(string property, params object[] parameters)
        {
            return null;
        }
    }
}
