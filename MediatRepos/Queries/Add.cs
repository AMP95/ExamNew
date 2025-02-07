using DTOs.Dtos;
using MediatR;
using Utilities.Interfaces;

namespace MediatRepos
{
    public class Add<TDto> : IRequest<IServiceResult<object>> where TDto : IDto
    {
        public TDto Value { get; set; }

        public Add(TDto value)
        {
            Value = value;
        }
    }

}
