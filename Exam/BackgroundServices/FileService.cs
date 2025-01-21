using DTOs.Dtos;
using Exam.Interfaces;
using MediatRepos;
using Microsoft.AspNetCore.Components.Web;
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
            _add = addService;
            _get = getService;
            _update = updateService;
            _hostEnvironment = hostEnvironment;
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
                                    result = await Get(item.FileId, stoppingToken);
                                    break;
                                case FileMethod.Delete:
                                    result = await Delete(item.FileId, stoppingToken);
                                    break;
                                case FileMethod.Add:
                                    result = await Add(item.Files, stoppingToken);
                                    break;
                                case FileMethod.Update:
                                    result = await Update(item.FileId, item.Files, stoppingToken);
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

        private async Task<ServiceResult> Get(Guid id, CancellationToken stoppingToken)
        {
            ServiceResult getResult = new ServiceResult()
            {
                ResultStatusCode = 404,
                ResultErrorMessage = "Файл не найден"
            };

            Guid requestId = await _get.Add(new GetId<FileDto>(id));

            while (_statusService.GetStatus(requestId).Result != RequestStatus.Done || stoppingToken.IsCancellationRequested) 
            { 
                Task.Delay(50).Wait();
            }

            ServiceResult result = await _resultService.GetResult(requestId);
            
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
                    Result = formFile,
                    ResultStatusCode = 200,
                };
            }

            return getResult;
        }

        private async Task<ServiceResult> Add(IFormFileCollection files, CancellationToken stoppingToken)
        {
            ServiceResult addResult = new ServiceResult()
            {
                ResultStatusCode = 500,
                ResultErrorMessage = "Не удалось добавить файл"
            };

            List<Guid> guids = new List<Guid>();

            if (files != null && files.Any()) 
            {
                foreach (IFormFile formFile in files) 
                {
                    string name = formFile.FileName;
                    string ext = Path.GetExtension(name);
                    string saveName = Path.Combine("Files", $"{Guid.NewGuid()}{ext}");
                    string path = Path.Combine(_hostEnvironment.WebRootPath, saveName); 

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await formFile.CopyToAsync(fileStream, stoppingToken);
                    }

                    FileDto dto = new FileDto() { Name = name, SaveName = saveName };

                    Guid requestGuid = await _add.Add(new Add<FileDto>(dto));

                    while (_statusService.GetStatus(requestGuid).Result != RequestStatus.Done || stoppingToken.IsCancellationRequested) 
                    { 
                        Task.Delay(50).Wait();
                    }

                    ServiceResult result = await _resultService.GetResult(requestGuid);

                    if (result.ResultStatusCode == 200) 
                    {
                        guids.Add((Guid)result.Result);
                    }
                }
            }

            if (guids.Any())
            {
                addResult = new ServiceResult()
                {
                    Result = guids,
                    ResultStatusCode = 200,
                };
            }

            return addResult;
        }

        private async Task<ServiceResult> Delete(Guid fileId, CancellationToken stoppingToken)
        {
            ServiceResult deleteResult = new ServiceResult()
            {
                ResultStatusCode = 500,
                ResultErrorMessage = "Не удалось удалить файл"
            };

            Guid requestId = await _get.Add(new GetId<FileDto>(fileId));

            while (_statusService.GetStatus(requestId).Result != RequestStatus.Done || stoppingToken.IsCancellationRequested)
            {
                Task.Delay(50).Wait();
            }

            ServiceResult getrResult = await _resultService.GetResult(requestId);

            if (getrResult.ResultStatusCode == 200) 
            {
                FileDto fileDto = getrResult.Result as FileDto;

                requestId = await _update.Add(new Delete<FileDto>(fileId));

                while (_statusService.GetStatus(requestId).Result != RequestStatus.Done || stoppingToken.IsCancellationRequested)
                {
                    Task.Delay(50).Wait();
                }

                ServiceResult result = await _resultService.GetResult(requestId);

                if (result.ResultStatusCode == 200)
                {
                    deleteResult = new ServiceResult()
                    {
                        Result = true,
                        ResultStatusCode = 200,
                    };

                    try
                    {
                        File.Delete(Path.Combine(_hostEnvironment.WebRootPath, fileDto.SaveName));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }
                }
            }

            return deleteResult;
        }

        private async Task<ServiceResult> Update(Guid fileId, IFormFileCollection files, CancellationToken stoppingToken)
        {
            ServiceResult updateResult = new ServiceResult()
            {
                ResultStatusCode = 500,
                Result = false,
                ResultErrorMessage = "Не удалось обновить файл"
            };

            Guid requestId = await _get.Add(new GetId<FileDto>(fileId));

            while (_statusService.GetStatus(requestId).Result != RequestStatus.Done || stoppingToken.IsCancellationRequested)
            {
                Task.Delay(50).Wait();
            }

            ServiceResult getrResult = await _resultService.GetResult(requestId);

            if (getrResult.ResultStatusCode == 200)
            {
                FileDto fileDto = getrResult.Result as FileDto;

                try
                {
                    File.Delete(Path.Combine(_hostEnvironment.WebRootPath, fileDto.SaveName));

                    string name = files.First().FileName;
                    string ext = Path.GetExtension(name);
                    string saveName = Path.Combine("Files", $"{Guid.NewGuid()}.{ext}");
                    string path = Path.Combine(_hostEnvironment.WebRootPath, saveName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await files.First().CopyToAsync(fileStream, stoppingToken);
                    }

                    fileDto.SaveName = saveName;

                    requestId = await _update.Add(new Update<FileDto>(fileDto));

                    while (_statusService.GetStatus(requestId).Result != RequestStatus.Done || stoppingToken.IsCancellationRequested)
                    {
                        Task.Delay(50).Wait();
                    }

                    ServiceResult result = await _resultService.GetResult(requestId);

                    if (result.ResultStatusCode == 200) 
                    {
                        updateResult = new ServiceResult()
                        {
                            ResultStatusCode = 200,
                            Result = true,
                        };
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }

            return updateResult;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _requests.Clear();
            return base.StopAsync(cancellationToken);
        }

       
    }
}
