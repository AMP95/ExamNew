using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq.Expressions;

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

    public abstract class GetFilterModelService<TModel> : IRequestHandler<GetFilter<TModel>, object>  where TModel : BaseEntity
    {
        protected IRepository _repository;
        protected ILogger<GetFilterModelService<TModel>> _logger;

        public GetFilterModelService(IRepository repository, ILogger<GetFilterModelService<TModel>> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<object> Handle(GetFilter<TModel> request, CancellationToken cancellationToken)
        {
            Expression<Func<TModel, bool>> filter = GetFilter(request.PropertyName, request.Params);
            return await Get(filter);
        }

        protected abstract Expression<Func<TModel, bool>> GetFilter(string property, params object[] parameters);

        protected abstract Task<object> Get(Expression<Func<TModel, bool>> filter);
    }

    public abstract class GetRangeModelService<TModel> : IRequestHandler<GetRange<TModel>, object> where TModel : BaseEntity
    {
        protected IRepository _repository;
        protected ILogger<GetRangeModelService<TModel>> _logger;

        public GetRangeModelService(IRepository repository, ILogger<GetRangeModelService<TModel>> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<object> Handle(GetRange<TModel> request, CancellationToken cancellationToken)
        {
            return await Get(request.Start, request.End);
        }

        protected abstract Task<object> Get(int start, int end);
    }
}
