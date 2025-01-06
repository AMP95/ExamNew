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

    public class GetMainId<TModel> : IRequest<object>
    {
        public Guid Id { get; set; }

        public GetMainId(Guid id)
        {
            Id = id;
        }
    }

    public class Search<TModel> : IRequest<object>
    {
        public string Name { get; set; }

        public Search(string name)
        {
            Name = name;
        }
    }

    public class GetRange<TModel> : IRequest<object>
    {
        public int Start { get; set; }
        public int End { get; set; }

        public GetRange(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}
