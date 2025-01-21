namespace Exam.Interfaces
{

    public interface IDownloadService
    {
        Task<Guid> Add(Guid guid);
    }
}
