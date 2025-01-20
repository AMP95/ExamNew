namespace Exam.Interfaces
{
    public enum FileMethod
    {
        Get,
        Add,
        Delete,
        Update
    }

    public interface IFileService
    {
        Task<Guid> Add(FileMethod method, Guid guid, IFormFileCollection files = null);
    }
}
