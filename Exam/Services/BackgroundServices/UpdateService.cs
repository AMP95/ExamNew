using Exam.Interfaces;
using MediatR;
using System.Collections.Concurrent;

namespace Exam.BackgroundServices
{
    public class UpdateService : BackgroundService, IUpdateService
    {
        private ConcurrentQueue<UpdateServiceQueueItem> _requests;
        private IRequestStatusService _statusService;
        private IResultService _resultService;
        private IMediator _mediator;
        private ILogger<UpdateService> _logger;

        public UpdateService(IRequestStatusService status, 
                             IResultService result, 
                             IMediator mediator, 
                             ILogger<UpdateService> logger)
        {
            _requests = new ConcurrentQueue<UpdateServiceQueueItem>();
            _statusService = status;
            _resultService = result;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Guid> Add(IRequest<bool> request)
        {
            Guid id = Guid.NewGuid();

            try
            {
                _requests.Enqueue(new UpdateServiceQueueItem() 
                { 
                    RequestId = id,
                    Request = request
                });
                _statusService.UpdateStatus(id, RequestStatus.Created);
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
                    if (_requests.TryDequeue(out UpdateServiceQueueItem item))
                    {
                        try
                        {
                            _statusService.UpdateStatus(item.RequestId, RequestStatus.InProgress);

                            bool result = await _mediator.Send(item.Request);

                            int code = 200;
                            string message = string.Empty;

                            if (!result)
                            {
                                code = 500;
                                message = "Внтренняя ошибка сервера";
                            }

                            _resultService.AddResult(item.RequestId, new ServiceResult() 
                            { 
                               CreateTime = DateTime.Now,
                               Result = result,
                               ResultStatusCode = code,
                               ResultErrorMessage = message
                            });
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
