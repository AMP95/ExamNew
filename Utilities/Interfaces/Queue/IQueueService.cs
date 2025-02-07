namespace Utilities.Interfaces
{
    public interface IQueueService<T>
    {
        Guid Enqueue(T request);
    }
}
