using DTOs.Dtos;
using MediatR;
using Utilities.Interfaces;

namespace MediatRepos
{
    public class Validate : IRequest<IServiceResult<object>> 
    { 
        public UserDto Logist { get; set; }

        public Validate(UserDto logist)
        {   
            Logist = logist;
        }
    }

}
