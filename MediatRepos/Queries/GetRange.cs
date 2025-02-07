using DTOs.Dtos;
using MediatR;
using Utilities.Interfaces;

namespace MediatRepos
{
    public class GetRange<TDto> : IRequest<IServiceResult<object>> where TDto : IDto
    {
        public int Start { get; set; }
        public int End { get; set; }

        public GetRange(int start, int end)
        {
            Start = start;
            End = end;
        }
    }

}
