using Exam.BackgroundServices;
using MediatR;
using System.Collections.Concurrent;
using Utilities.Interfaces;

namespace Exam.Services.BackgroundServices
{
    public class QueueService : BackgroundService, IQueueService<IRequest<IServiceResult<object>>>
    {
        #region Private

        private ConcurrentQueue<QueueItem> _requests;
        private ILogger<QueueService> _logger;
        private IStatusService _statusService;
        private IResultService _resultService;
        private IMediator _mediator;

        #endregion Private

        public QueueService(IStatusService status,
                            IResultService result,
                            IMediator mediator,
                            ILogger<QueueService> logger)
        {
            _requests = new ConcurrentQueue<QueueItem>();
            _statusService = status;
            _resultService = result;
            _mediator = mediator;
            _logger = logger;
        }


        public Guid Enqueue(IRequest<IServiceResult<object>> request)
        {
            Guid id = Guid.NewGuid();

            try
            {
                _requests.Enqueue(new QueueItem()
                {
                    Id = id,
                    Request = request
                });
                _statusService.UpdateStatus(id, Status.Created);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return id;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (_requests.TryDequeue(out QueueItem item))
                    {
                        try
                        {
                            _statusService.UpdateStatus(item.Id, Status.InProgress);

                            _resultService.AddResult(item.Id, await _mediator.Send(item.Request));
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message);
                        }
                    }
                    else
                    {
                        Task.Delay(50).Wait();
                    }
                }
            }, stoppingToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _requests.Clear();
            return base.StopAsync(cancellationToken);
        }
    }
}
