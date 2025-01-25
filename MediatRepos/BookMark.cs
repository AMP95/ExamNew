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
            new BookMarkDto(){ Name = "Маршрут", InsertView = "{{Route}}" },
            new BookMarkDto(){ Name = "Вес", InsertView = "{{Weight}}" },
            new BookMarkDto(){ Name = "Объем", InsertView = "{{Volume}}" },
            new BookMarkDto(){ Name = "Погрузка", InsertView = "{{LoadingPoint}}" },
            new BookMarkDto(){ Name = "Выгрузка", InsertView = "{{UnloadingPoint}}" },
            new BookMarkDto(){ Name = "Оплата", InsertView = "{{Payment}}" },
            new BookMarkDto(){ Name = "Условия оплаты", InsertView = "{{PayConditions}}" },
            new BookMarkDto(){ Name = "Перевозчик", InsertView = "{{Carrier}}" },
            new BookMarkDto(){ Name = "Водитель", InsertView = "{{Driver}}" },
            new BookMarkDto(){ Name = "ТС", InsertView = "{{Vehicle}}" },
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
