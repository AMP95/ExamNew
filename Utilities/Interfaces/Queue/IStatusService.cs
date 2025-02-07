namespace Utilities.Interfaces
{
    public enum Status
    {
        Created,
        InProgress,
        Done,
        Unknown
    }

    public interface IStatusService
    {
        Task<Status> GetStatus(Guid id);
        Task RemoveStatus(Guid id);
        Task UpdateStatus(Guid id, Status status);
    }
}