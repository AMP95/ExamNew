namespace Utilities.Interfaces
{
    public enum Status
    {
        Created,
        InProgress,
        Done
    }

    public interface IStatusService
    {
        Task<IServiceResult<Status>> GetStatus(Guid id);
        Task RemoveStatus(Guid id);
        Task UpdateStatus(Guid id, Status status);
    }
}