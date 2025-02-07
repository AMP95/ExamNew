namespace Utilities.Interfaces
{
    public interface IQueueService<T>
    {
        Task<IServiceResult<Guid>> Enqueue(T request);
    }
}
