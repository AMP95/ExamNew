using DTOs.Dtos;
using Exam.Interfaces;
using MediatorServices.Abstract;
using MediatRepos;
using System.Collections.Concurrent;

namespace Exam.BackgroundServices
{
    public class DownloadService : BackgroundService, IDownloadService
    {
        private ConcurrentQueue<FileServiceQueueItem> _requests;
        private IRequestStatusService _statusService;
        private IResultService _resultService;
        private ILogger<DownloadService> _logger;
        private IGetService _get;
        private IFileManager _fileManager;

        public DownloadService(IRequestStatusService status,
                               IResultService result,
                               ILogger<DownloadService> logger,
                               IGetService getService,
                               IFileManager fileManager)
        {
            _requests = new ConcurrentQueue<FileServiceQueueItem>();
            _statusService = status;
            _resultService = result;
            _logger = logger;
            _get = getService;
        }

        public async Task<Guid> Add(Guid fileId)
        {
            Guid id = Guid.NewGuid();

            try
            {
                _requests.Enqueue(new FileServiceQueueItem()
                {
                    RequestId = id,
                    FileId = fileId,
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
                    if (_requests.TryDequeue(out FileServiceQueueItem item))
                    {
                        try
                        {
                            _statusService.UpdateStatus(item.RequestId, RequestStatus.InProgress);

                            Guid requestId = await _get.Add(new GetId<FileDto>(item.FileId));

                            while (_statusService.GetStatus(requestId).Result != RequestStatus.Done && !stoppingToken.IsCancellationRequested)
                            {
                                Task.Delay(50).Wait();
                            }

                            ServiceResult currentResult = null;

                            ServiceResult getResult = await _resultService.GetResult(requestId);

                            if (getResult.ResultStatusCode == 200)
                            {
                                FileDto fileDto = getResult.Result as FileDto;
                                string extensions = Path.GetExtension(fileDto.FileName);
                                string filePath = Path.Combine(fileDto.SubFolder, $"{fileDto.Id}{extensions}");

                                IFormFile formFile = await _fileManager.GetFile(filePath, fileDto.FileName);
                                if (formFile != null)
                                {
                                    currentResult = new ServiceResult()
                                    {
                                        Result = formFile,
                                        ResultStatusCode = 200,
                                    };
                                }
                            }

                            if (currentResult == null) 
                            {
                                currentResult = new ServiceResult()
                                {
                                    ResultStatusCode = 404,
                                    ResultErrorMessage = "Файл не найден"
                                };
                            }

                            _resultService.AddResult(item.RequestId, currentResult);
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
