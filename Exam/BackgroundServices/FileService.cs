using DTOs.Dtos;
using Exam.Interfaces;
using MediatRepos;
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;

namespace Exam.BackgroundServices
{
    public class FileService : BackgroundService, IFileService
    {
        private ConcurrentQueue<FileServiceQueueItem> _requests;
        private IRequestStatusService _statusService;
        private IResultService _resultService;
        private ILogger<FileService> _logger;
        private IAddService _add;
        private IGetService _get;
        private IUpdateService _update;
        private IWebHostEnvironment _hostEnvironment;

        public FileService(IRequestStatusService status,
                           IResultService result,
                           ILogger<FileService> logger,
                           IAddService addService,
                           IGetService getService,
                           IUpdateService updateService,
                           IWebHostEnvironment hostEnvironment)
        {
            _requests = new ConcurrentQueue<FileServiceQueueItem>();
            _statusService = status;
            _resultService = result;
            _logger = logger;
        }

        public async Task<Guid> Add(FileMethod method, Guid guid, IFormFileCollection files = null)
        {
            Guid id = Guid.NewGuid();

            try
            {
                _requests.Enqueue(new FileServiceQueueItem()
                {
                    RequestId = id,
                    Method = method,
                    FileId = guid,
                    Files = files
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

                            ServiceResult result = null;

                            switch (item.Method) 
                            {
                                case FileMethod.Get:
                                    result = await Get(item.FileId);
                                    break;
                                case FileMethod.Delete:
                                    result = await Delete(item.FileId);
                                    break;
                                case FileMethod.Add:
                                    result = await Add(item.Files);
                                    break;
                                case FileMethod.Update:
                                    result = await Update(item.FileId, item.Files);
                                    break;
                            }

                            _resultService.AddResult(item.RequestId, result);
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

            

            throw new NotImplementedException();
        }

        private async Task<ServiceResult> Get(Guid id)
        {
            Guid requestId = await _get.Add(new GetId<FileDto>(id));

            while (_statusService.GetStatus(requestId).Result != RequestStatus.Done) 
            { 
                Task.Delay(50).Wait();
            }

            ServiceResult result = await _resultService.GetResult(requestId);

            ServiceResult getResult = null;
            if (result.ResultStatusCode == 200)
            {
                FileDto dto = result.Result as FileDto;

                FormFile formFile;
                using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(Path.Combine(_hostEnvironment.WebRootPath, "Files", dto.SaveName)).ToArray()))
                {
                    formFile = new FormFile(stream, 0, stream.Length, null, dto.Name);
                }

                getResult = new ServiceResult()
                {
                    CreateTime = DateTime.Now,
                    Result = formFile,
                    ResultStatusCode = 200,
                };
            }
            else 
            {
                getResult = new ServiceResult()
                {
                    CreateTime = DateTime.Now,
                    ResultStatusCode = result.ResultStatusCode,
                    ResultErrorMessage = result.ResultErrorMessage
                };
            }

            return getResult;
        }

        private async Task<ServiceResult> Add(IFormFileCollection files)
        {
            throw new NotImplementedException();
        }

        private async Task<ServiceResult> Delete(Guid fileId)
        {
            throw new NotImplementedException();
        }

        private async Task<ServiceResult> Update(Guid fileId, IFormFileCollection files)
        {
            throw new NotImplementedException();
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _requests.Clear();
            return base.StopAsync(cancellationToken);
        }

       
    }
}
