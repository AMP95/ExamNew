using DTOs;
using Microsoft.Extensions.Logging;
using Models;
using Models.Sub;
using System.Linq.Expressions;

namespace MediatorServices
{
    public class GetFilteredRouteService : GetFilteredModelService<RoutePoint>
    {
        public GetFilteredRouteService(IRepository repository, ILogger<GetIdModelService<RoutePoint>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Expression<Func<RoutePoint, bool>> filter)
        {
            IEnumerable<RoutePoint> points = await _repository.Get(filter);
            List<RoutePointDto> dtos = new List<RoutePointDto>();

            foreach (var point in points)
            {
                RoutePointDto dto = new RoutePointDto()
                {
                    Id = point.Id,
                    Route = point.Route,
                    Address = point.Address,
                    Phones = point.Phones.Split(';').ToList(),
                    Type = (LoadPointType)point.Type,
                    Side = (LoadingSide)point.Side,
                };

                dtos.Add(dto);
            }

            return dtos;
        }
    }
}
