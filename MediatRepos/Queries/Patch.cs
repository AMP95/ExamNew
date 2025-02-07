using DTOs.Dtos;
using MediatR;
using Utilities.Interfaces;

namespace MediatRepos
{
    public class Patch<TDto> : IRequest<IServiceResult<object>> where TDto : IDto
    {
        public Guid Id { get; set; }
        public KeyValuePair<string, object>[] Updates { get; set; }

        public Patch(Guid id, KeyValuePair<string, object>[] updates)
        {
            Id = id;
            Updates = updates;
        }
    }

}
