using MediatR;
using Utilities.Interfaces;

namespace MediatRepos
{
    public class GetFile : IRequest<IServiceResult<object>> 
    {
        public Guid Id { get; set; }

        public GetFile(Guid id)
        {
            Id = id;
        }
    }

}
