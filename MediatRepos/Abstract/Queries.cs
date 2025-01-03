using MediatR;
using System.Linq.Expressions;

namespace MediatRepos
{
    public class Update<TDto> : IRequest<bool>
    {
        public TDto Value { get; set; }

        public Update(TDto value)
        {
            Value = value;
        }
    }

    public class Add<TDto> : IRequest<bool>
    {
        public TDto Value { get; set; }

        public Add(TDto value)
        {
            Value = value;
        }
    }

    public class Delete<T> : IRequest<bool>
    {
        public Guid Id { get; set; }

        public Delete(Guid id)
        {
            Id = id;
        }
    }

    public class GetId<TModel> : IRequest<object>
    {
        public Guid Id { get; set; }

        public GetId(Guid id)
        {
            Id = id;
        }
    }

    public class GetFiltered<TModel> : IRequest<object>
    {
        public Expression<Func<TModel, bool>> Filter { get; set; }

        public GetFiltered(Expression<Func<TModel, bool>> filter = null)
        {
            Filter = filter;
        }
    }
}
