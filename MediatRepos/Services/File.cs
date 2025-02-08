using DTOs;
using DTOs.Dtos;
using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.Main;
using System.Linq.Expressions;
using Utilities.Interfaces;

namespace MediatorServices
{
    internal static class FileConverter
    {
        public static FileDto Convert(Models.Sub.File file)
        {
            FileDto dto = new FileDto()
            {
                Id = file.Id,
                FileNameWithExtencion = file.ViewNameWithExtencion,
                Catalog = Path.GetFileName(Path.GetDirectoryName(file.FullFilePath)),
                DtoId = file.EntityId,
            };

            switch (file.EntityType)
            {
                case nameof(Carrier):
                    dto.DtoType = nameof(CarrierDto);
                    break;
                case nameof(Driver):
                    dto.DtoType = nameof(DriverDto);
                    break;
                case nameof(Vehicle):
                    dto.DtoType = nameof(VehicleDto);
                    break;
                case nameof(Contract):
                    dto.DtoType = nameof(ContractDto);
                    break;
                case nameof(Template):
                    dto.DtoType = nameof(TemplateDto);
                    break;
            }

            return dto;
        }
    }

    public class GetIdFileService : IRequestHandler<GetId<FileDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public GetIdFileService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(GetId<FileDto> request, CancellationToken cancellationToken)
        {
            Models.Sub.File file = await _repository.GetById<Models.Sub.File>(request.Id);

            if (file == null)
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Файл не найден"
                };
            }
            else 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = true,
                    Result = FileConverter.Convert(file)
                };
            }
        }
    }

    public class FileSendResult 
    { 
        public byte[] Data { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }

    public class DownloadFileService : IRequestHandler<GetFile, IServiceResult<object>>
    {
        protected IRepository _repository;
        protected ILogger<DownloadFileService> _logger;
        protected IFileManager _fileManager;

        public DownloadFileService(IRepository repository,
                                   ILogger<DownloadFileService> logger,
                                   IFileManager fileManager)
        {
            _repository = repository;
            _logger = logger;
            _fileManager = fileManager;
        }

        public async Task<IServiceResult<object>> Handle(GetFile request, CancellationToken cancellationToken)
        {
            Models.Sub.File file = await _repository.GetById<Models.Sub.File>(request.Id);

            if (file == null)
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Файл не найден"
                };
            }
            else
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = true,
                    Result = new FileSendResult()
                    {
                        FileName = file.ViewNameWithExtencion,
                        ContentType = "application/octet-stream",
                        Data = await _fileManager.GetFile(file.FullFilePath, file.ViewNameWithExtencion)
                    }
                };
            }
        }
    }

    public class GetFilterFileService : IRequestHandler<GetFilter<FileDto>, IServiceResult<object>>
    {
        protected IRepository _repository;
        protected ILogger<GetFilterFileService> _logger;

        public GetFilterFileService(IRepository repository, ILogger<GetFilterFileService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IServiceResult<object>> Handle(GetFilter<FileDto> request, CancellationToken cancellationToken)
        {
            Expression<Func<Models.Sub.File, bool>> filter = GetFilter(request.PropertyName, request.Params);

            IEnumerable<Models.Sub.File> files = await _repository.Get(filter);
            List<FileDto> dtos = new List<FileDto>();

            foreach (var file in files)
            {
                dtos.Add(FileConverter.Convert(file));
            }

            return new MediatorServiceResult()
            {
                IsSuccess = true,
                Result = dtos
            };
        }


        protected Expression<Func<Models.Sub.File, bool>> GetFilter(string property, params object[] parameters)
        {
            Expression<Func<Models.Sub.File, bool>> filter = null;

            try
            {
                if (property == nameof(FileDto.DtoId) && Guid.TryParse(parameters[0].ToString(), out var entityId)) 
                { 
                    filter = f => f.EntityId == entityId;
                }
            }
            catch (Exception ex)
            {
                filter = d => false;
                _logger.LogError(ex, ex.Message);
            }

            return filter;
        }
    }

    public class DeleteFileService : IRequestHandler<Delete<FileDto>, IServiceResult<object>>
    {
        private IRepository _repository;
        private IFileManager _fileManager;

        public DeleteFileService(IRepository repository, IFileManager fileManager)
        {
            _repository = repository;
            _fileManager = fileManager;
        }

        public async Task<IServiceResult<object>> Handle(Delete<FileDto> request, CancellationToken cancellationToken)
        {
            Models.Sub.File file = await _repository.GetById<Models.Sub.File>(request.Id);

            if (file == null)
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Файл не найден"
                };
            }

            if (await _repository.Remove<Models.Sub.File>(request.Id))
            {
                _fileManager.RemoveFile(file.FullFilePath);

                return new MediatorServiceResult()
                {
                    IsSuccess = true,
                    Result = true,
                };
            }
            else 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Не удалось удалить файл"
                };
            }
        }
    }

    public class AddFileService : IRequestHandler<Add<FileDto>, IServiceResult<object>>
    {
        private IFileManager _fileManager;
        private IRepository _repository;

        public AddFileService(IRepository repository, IFileManager fileManager)
        {
            _repository = repository;
            _fileManager = fileManager;
        }

        public async Task<IServiceResult<object>> Handle(Add<FileDto> request, CancellationToken cancellationToken)
        {
            FileDto dto = request.Value;

            string extencion = Path.GetExtension(dto.FileNameWithExtencion);
            string entityCatalog = string.Empty;

            switch (dto.DtoType)
            {
                case nameof(CarrierDto):
                    entityCatalog = nameof(Carrier);
                    break;
                case nameof(CompanyDto):
                    entityCatalog = nameof(Company);
                    break;
                case nameof(DriverDto):
                    entityCatalog = nameof(Driver);
                    break;
                case nameof(VehicleDto):
                    entityCatalog = nameof(Vehicle);
                    break;
                case nameof(ContractDto):
                    entityCatalog = nameof(Contract);
                    break;
                case nameof(TemplateDto):
                    entityCatalog = nameof(Template);
                    break;
            }

            string fullSavePath = Path.Combine(entityCatalog, dto.Catalog, $"{Guid.NewGuid()}{extencion}");

            if (await _fileManager.SaveFile(fullSavePath, dto.FileNameWithExtencion))
            {
                Models.Sub.File entityFile = new Models.Sub.File()
                {
                    ViewNameWithExtencion = dto.FileNameWithExtencion,
                    FullFilePath = fullSavePath,
                    EntityId = dto.DtoId,
                    EntityType = entityCatalog,
                };

                Guid id = await _repository.Add(entityFile);

                if (id != Guid.Empty) 
                {
                    return new MediatorServiceResult()
                    {
                        IsSuccess = true,
                        Result = id
                    };
                }
            }

            return new MediatorServiceResult()
            {
                IsSuccess = false,
                ErrorMessage = "Не удалось сохранить файл"
            };
        }
    }
}
