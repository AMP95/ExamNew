namespace Utilities.Interfaces
{
    public interface IQueueService<T>
    {
        Task<Guid> Enqueue(T request);
    }
}
