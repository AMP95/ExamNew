using DTOs.Dtos;
using MediatR;
using Models;

namespace MediatRepos
{
    public abstract class AddModelService<TDto> : IRequestHandler<Add<TDto>, Guid> where TDto : IDto
    {
        protected IRepository _repository;
        public AddModelService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(Add<TDto> request, CancellationToken cancellationToken)
        {
            if (request.Value == null)
            {
                return Guid.Empty;
            }
            return await Add(request.Value);
        }

        protected abstract Task<Guid> Add(TDto dto);
    }
}
