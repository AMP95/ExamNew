using MediatR;

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
        public int Id { get; set; }

        public Delete(int id)
        {
            Id = id;
        }
    }

    public class GetAll<TModel> : IRequest<object>
    {

    }

    public class GetId<TModel> : IRequest<object>
    {
        public int Id { get; set; }

        public GetId(int id)
        {
            Id = id;
        }
    }

    public class GetCondition<TModel> : IRequest<object>
    {
        public Func<TModel, bool> Condition { get; set; }

        public GetCondition(Func<TModel,bool> condition)
        {
            Condition = condition;
        }
    }
}
