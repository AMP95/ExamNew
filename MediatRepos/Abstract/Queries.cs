using Azure.Core;
using DTOs;
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

    public class SetContractStatus : IRequest<bool> 
    {
        public Guid ContractId { get; set; }
        public ContractStatus ContractStatus { get; set; }

        public SetContractStatus(Guid id, ContractStatus status)
        {
            ContractId = id;
            ContractStatus = status;
        }
    }

    public class GetFilter<TModel> : IRequest<object>
    { 
        public string PropertyName { get; set; }

        public object[] Params { get; set; }

        public GetFilter(string propertyName, params string[] parameters)
        {
            PropertyName = propertyName;
            Params = parameters;
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
