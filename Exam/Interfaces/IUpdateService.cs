using MediatR;

namespace Exam.Interfaces
{
    public interface IUpdateService
    {
        Task<Guid> Add(IRequest<bool> request);
    }
}
