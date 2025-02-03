using Exam.Interfaces;
using MediatR;

namespace Exam.BackgroundServices
{
    public class ServiceQueueItem
    {
        public Guid RequestId { get; set; }
    }

    public class GetServiceQueueItem : ServiceQueueItem
    {
        public IRequest<object> Request { get; set; }
    }

    public class UpdateServiceQueueItem : ServiceQueueItem
    {
        public IRequest<bool> Request { get; set; }
    }

    public class AddServiceQueueItem : ServiceQueueItem
    {
        public IRequest<Guid> Request { get; set; }
    }

    

    public class FileServiceQueueItem : ServiceQueueItem
    {
        public Guid FileId { get; set; }
    }
}
