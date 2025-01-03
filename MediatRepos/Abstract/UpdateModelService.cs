using MediatR;
using Models;

namespace MediatRepos
{
    public abstract class UpdateModelService<TDto> : IRequestHandler<Update<TDto>, bool> where TDto : class
    {
        protected IRepository _repository;
        public UpdateModelService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(Update<TDto> request, CancellationToken cancellationToken)
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
