using DTOs.Dtos;
using MediatR;
using Utilities.Interfaces;

namespace MediatRepos
{
    public class Validate : IRequest<IServiceResult<object>> 
    { 
        public LogistDto Logist { get; set; }

        public Validate(LogistDto logist)
        {   
            Logist = logist;
        }
    }

}
