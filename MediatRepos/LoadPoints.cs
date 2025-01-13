using DTOs;
using Microsoft.Extensions.Logging;
using Models;
using Models.Sub;

namespace MediatorServices
{
    public class GetRangeRoutePoint : GetRangeModelService<RoutePoint>
    {
        public GetRangeRoutePoint(IRepository repository, ILogger<GetRangeModelService<RoutePoint>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(int start, int end)
        {
            IEnumerable<RoutePoint> points = await _repository.Get<RoutePoint>(r => r.Type == (int)LoadPointType.Upload);
            List<RoutePointDto> dtos = new List<RoutePointDto>();

            foreach (var point in points)
            {
                RoutePointDto dto = new RoutePointDto()
                {
                    Id = point.Id,
                    Route = point.Route,
                    DateAndTime = point.DateAndTime,
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
