using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;

namespace MediatorServices
{
    public abstract class GetIdModelService<TModel> : IRequestHandler<GetId<TModel>, object> where TModel : BaseEntity
    {
        protected IRepository _repository;
        protected ILogger<GetIdModelService<TModel>> _logger;

        public GetIdModelService(IRepository repository, ILogger<GetIdModelService<TModel>> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<object> Handle(GetId<TModel> request, CancellationToken cancellationToken)
        {
            return await Get(request.Id);
        }
        protected abstract Task<object> Get(Guid id);

    }
}
