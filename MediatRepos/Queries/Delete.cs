using DTOs.Dtos;
using MediatR;
using Utilities.Interfaces;

namespace MediatRepos
{
    public class Delete<TDto> : IRequest<IServiceResult<object>> where TDto : IDto
    {
        public Guid Id { get; set; }

        public Delete(Guid id)
        {
            Id = id;
        }
    }

}
