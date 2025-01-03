using MediatR;
using Models;

namespace MediatRepos
{
    public abstract class DeleteModelService<T> : IRequestHandler<Delete<T>, bool> where T : BaseEntity
    {
        protected IRepository _repository;
        public DeleteModelService(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(Delete<T> request, CancellationToken cancellationToken)
        {
            return await _repository.Remove<T>(request.Id);
        }
    }
}
