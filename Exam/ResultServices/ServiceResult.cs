namespace Exam.BackgroundServices
{
    public class ServiceResult
    {
        public object Result { get; set; }
        public int ResultStatusCode { get; set; }
        public string ResultErrorMessage { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
