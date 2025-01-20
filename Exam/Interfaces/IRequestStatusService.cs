
using Exam.ResultServices;

namespace Exam.Interfaces
{
    public enum RequestStatus
    {
        Created,
        InProgress,
        Done,
        Unknown
    }

    public interface IRequestStatusService
    {
        Task<RequestStatus> GetStatus(Guid id);
        Task RemoveStatus(Guid id);
        Task UpdateStatus(Guid id, RequestStatus status);
    }
}