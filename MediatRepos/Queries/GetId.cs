using DTOs.Dtos;
using MediatR;
using Utilities.Interfaces;

namespace MediatRepos
{
    public class GetId<TDto> : IRequest<IServiceResult<object>> where TDto : IDto
    {
        public Guid Id { get; set; }

        public GetId(Guid id)
        {
            Id = id;
        }
    }

}
