namespace Utilities.Interfaces
{
    public interface IResultService
    {
        Task AddResult(Guid id, IServiceResult<object> result);
        Task<IServiceResult<object>> GetResult(Guid id);
    }
}
