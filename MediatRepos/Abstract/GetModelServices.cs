using DTOs.Dtos;
using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;

namespace MediatorServices
{
    public abstract class GetIdModelService<TDto> : IRequestHandler<GetId<TDto>, object> where TDto : IDto
    {
        protected IRepository _repository;
        protected ILogger<GetIdModelService<TDto>> _logger;

        public GetIdModelService(IRepository repository, ILogger<GetIdModelService<TDto>> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<object> Handle(GetId<TDto> request, CancellationToken cancellationToken)
        {
            return await Get(request.Id);
        }
        protected abstract Task<object> Get(Guid id);

    }


    public abstract class GetRangeModelService<TDto> : IRequestHandler<GetRange<TDto>, object> where TDto : IDto
    {
        protected IRepository _repository;
        protected ILogger<GetRangeModelService<TDto>> _logger;

        public GetRangeModelService(IRepository repository, ILogger<GetRangeModelService<TDto>> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<object> Handle(GetRange<TDto> request, CancellationToken cancellationToken)
        {
            return await Get(request.Start, request.End);
        }

        protected abstract Task<object> Get(int start, int end);
    }
}
