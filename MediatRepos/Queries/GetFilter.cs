using DTOs.Dtos;
using MediatR;
using Utilities.Interfaces;

namespace MediatRepos
{
    public class GetFilter<TDto> : IRequest<IServiceResult<object>> where TDto : IDto
    { 
        public string PropertyName { get; set; }

        public object[] Params { get; set; }

        public GetFilter(string propertyName, params string[] parameters)
        {
            PropertyName = propertyName;
            Params = parameters;
        }
    }

}
