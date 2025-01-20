using MediatR;

namespace Exam.Interfaces
{
    public interface IAddService
    {
        Task<Guid> Add(IRequest<Guid> request);
    }
}
