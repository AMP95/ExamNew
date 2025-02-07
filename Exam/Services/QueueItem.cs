using MediatR;
using Utilities.Interfaces;

namespace Exam.BackgroundServices
{
    public class QueueItem
    {
        public Guid Id { get; set; }
        public IRequest<IServiceResult<object>> Request { get; set; } 
    }
}
