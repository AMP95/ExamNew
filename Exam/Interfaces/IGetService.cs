using MediatR;

namespace Exam.Interfaces
{
    public interface IGetService
    {
        Task<Guid> Add(IRequest<object> request);
    }
}
