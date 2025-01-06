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

    public abstract class GetMainIdModelService<TModel> : IRequestHandler<GetMainId<TModel>, object> where TModel : BaseEntity
    {
        protected IRepository _repository;
        protected ILogger<GetMainIdModelService<TModel>> _logger;

        public GetMainIdModelService(IRepository repository, ILogger<GetMainIdModelService<TModel>> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<object> Handle(GetMainId<TModel> request, CancellationToken cancellationToken)
        {
            return await Get(request.Id);
        }

        protected abstract Task<object> Get(Guid id);
    }

    public abstract class SearchModelService<TModel> : IRequestHandler<Search<TModel>, object> where TModel : BaseEntity
    {
        protected IRepository _repository;
        protected ILogger<SearchModelService<TModel>> _logger;

        public SearchModelService(IRepository repository, ILogger<SearchModelService<TModel>> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<object> Handle(Search<TModel> request, CancellationToken cancellationToken)
        {
            return await Get(request.Name);
        }

        protected abstract Task<object> Get(string name);
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
