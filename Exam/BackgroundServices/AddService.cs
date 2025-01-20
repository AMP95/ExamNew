using Exam.Interfaces;
using MediatR;
using System.Collections.Concurrent;

namespace Exam.BackgroundServices
{
    public class AddService : BackgroundService, IAddService
    {
        private ConcurrentQueue<AddServiceQueueItem> _requests;
        private IRequestStatusService _statusService;
        private IResultService _resultService;
        private IMediator _mediator;
        private ILogger<AddService> _logger;

        public AddService(IRequestStatusService status,
                          IResultService result,
                          IMediator mediator,
                          ILogger<AddService> logger)
        {
            _requests = new ConcurrentQueue<AddServiceQueueItem>();
            _statusService = status;
            _resultService = result;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Guid> Add(IRequest<Guid> request)
        {
            Guid id = Guid.NewGuid();

            try
            {
                _requests.Enqueue(new AddServiceQueueItem()
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
                    if (_requests.TryDequeue(out AddServiceQueueItem item))
                    {
                        try
                        {
                            _statusService.UpdateStatus(item.RequestId, RequestStatus.InProgress);

                            Guid result = await _mediator.Send(item.Request);

                            int code = 200;
                            string message = string.Empty;

                            if (result == Guid.Empty) 
                            {
                                code = 500;
                                message = "Не удалось добавить объект";
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
