namespace Exam.Interfaces
{
    public class ServiceResult
    {
        public object Result { get; set; }
        public int ResultStatusCode { get; set; }
        public string ResultErrorMessage { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public interface IResultService
    {
        Task AddResult(Guid id, ServiceResult result);
        Task<ServiceResult> GetResult(Guid id);
    }
}
