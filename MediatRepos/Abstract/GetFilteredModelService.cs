using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq.Expressions;

namespace MediatorServices
{
    public abstract class GetFilteredModelService<TModel> : IRequestHandler<GetFiltered<TModel>, object> where TModel : BaseEntity
    {
        protected IRepository _repository;
        protected ILogger<GetIdModelService<TModel>> _logger;

        public GetFilteredModelService(IRepository repository, ILogger<GetIdModelService<TModel>> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<object> Handle(GetFiltered<TModel> request, CancellationToken cancellationToken)
        {
            return await Get(request.Filter);
        }

        protected abstract Task<object> Get(Expression<Func<TModel, bool>> filter);
    }
}
