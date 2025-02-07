using DTOs.Dtos;
using MediatR;
using Utilities.Interfaces;

namespace MediatRepos
{
    public class Update<TDto> : IRequest<IServiceResult<object>> where TDto : IDto
    {
        public TDto Value { get; set; }

        public Update(TDto value)
        {
            Value = value;
        }
    }

}
