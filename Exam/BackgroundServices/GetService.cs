using Exam.ResultServices;
using MediatR;
using System.Collections.Concurrent;

namespace Exam.BackgroundServices
{
    public class GetService : BackgroundService
    {
        private ConcurrentQueue<GetServiceQueueItem> _requests;
        private ILogger<GetService> _logger;
        private RequestStatusService _statusService;
        private ResultService _resultService;
        private IMediator _mediator;

        public GetService(RequestStatusService status, ResultService result, IMediator mediator, ILogger<GetService> logger)
        {
            _requests = new ConcurrentQueue<GetServiceQueueItem>();
            _statusService = status;
            _resultService = result;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Guid> Add(IRequest<object> request)
        {
            Guid id = Guid.NewGuid();
            try
            {
                _requests.Enqueue(new GetServiceQueueItem() 
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
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (_requests.TryDequeue(out GetServiceQueueItem item))
                    {
                        try
                        {
                            _statusService.UpdateStatus(item.RequestId, RequestStatus.InProgress);

                            object result = await _mediator.Send(item.Request);

                            int code = result == null ? 404 : 200;
                            string message = result == null ? "Объект не найден" : string.Empty;

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
