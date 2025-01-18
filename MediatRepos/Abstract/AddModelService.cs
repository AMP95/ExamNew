using DTOs.Dtos;
using MediatR;
using Models;

namespace MediatRepos
{
    public abstract class AddModelService<TDto> : IRequestHandler<Add<TDto>, bool> where TDto : IDto
    {
        protected IRepository _repository;
        public AddModelService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(Add<TDto> request, CancellationToken cancellationToken)
        {
            if (request.Value == null)
            {
                return false;
            }
            return await Update(request.Value);
        }

        protected abstract Task<bool> Update(TDto dto);
    }
}
