using DTOs;
using DTOs.Dtos;
using MediatorServices.Abstract;
using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.Main;
using System.Linq.Expressions;

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
                    dto.DtoType = typeof(CarrierDto);
                    break;
                case nameof(Driver):
                    dto.DtoType = typeof(DriverDto);
                    break;
                case nameof(Vehicle):
                    dto.DtoType = typeof(VehicleDto);
                    break;
                case nameof(Contract):
                    dto.DtoType = typeof(ContractDto);
                    break;
                case nameof(ContractTemplate):
                    dto.DtoType = typeof(ContractTemplateDto);
                    break;
            }

            return dto;
        }
    }


    public class GetIdFileService : GetIdModelService<FileDto>
    {
        public GetIdFileService(IRepository repository, 
                                ILogger<GetIdModelService<FileDto>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            Models.Sub.File file = await _repository.GetById<Models.Sub.File>(id);
            if (file != null)
            {
                return FileConverter.Convert(file);
            }
            return null;
        }
    }

    public class GetFilterFileService : IRequestHandler<GetFilter<FileDto>, object>
    {
        protected IRepository _repository;
        protected ILogger<GetFilterFileService> _logger;

        public GetFilterFileService(IRepository repository, ILogger<GetFilterFileService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<object> Handle(GetFilter<FileDto> request, CancellationToken cancellationToken)
        {
            Expression<Func<Models.Sub.File, bool>> filter = GetFilter(request.PropertyName, request.Params);
            return await Get(filter);
        }

        protected async Task<object> Get(Expression<Func<Models.Sub.File, bool>> filter)
        {
            IEnumerable<Models.Sub.File> files = await _repository.Get<Models.Sub.File>(filter);
            List<FileDto> dtos = new List<FileDto>();

            foreach (var file in files)
            {
                dtos.Add(FileConverter.Convert(file));
            }

            return dtos;
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

    public class DeleteFileService : IRequestHandler<Delete<FileDto>, bool>
    {
        private IRepository _repository;
        private IFileManager _fileManager;

        public DeleteFileService(IRepository repository, IFileManager fileManager)
        {
            _repository = repository;
            _fileManager = fileManager;
        }

        public async Task<bool> Handle(Delete<FileDto> request, CancellationToken cancellationToken)
        {
            Models.Sub.File file = await _repository.GetById<Models.Sub.File>(request.Id);

            if (file != null)
            {
                if (await _repository.Remove<Models.Sub.File>(request.Id))
                {
                    _fileManager.RemoveFile(file.FullFilePath);
                    return true;
                }
            }
            return false;
        }
    }

    public class AddFileService : AddModelService<FileDto>
    {
        private IFileManager _fileManager;
        public AddFileService(IRepository repository, 
                              IFileManager fileManager) : base(repository)
        {
            _fileManager = fileManager;
        }

        protected override async Task<Guid> Add(FileDto dto)
        {
            string extencion = Path.GetExtension(dto.FileNameWithExtencion);
            string entityCatalog = string.Empty;

            switch (dto.DtoType.Name) 
            {
                case nameof(CarrierDto):
                    entityCatalog = nameof(Carrier);
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
                case nameof(ContractTemplateDto):
                    entityCatalog = nameof(ContractTemplate);
                    break;
            }

            string fullSavePath = Path.Combine(entityCatalog, dto.Catalog, $"{Guid.NewGuid()}{extencion}");

            if (await _fileManager.SaveFile(fullSavePath, dto.File)) 
            {
                Models.Sub.File entityFile = new Models.Sub.File()
                {
                    ViewNameWithExtencion = dto.FileNameWithExtencion,
                    FullFilePath = fullSavePath,
                    EntityId = dto.DtoId,
                    EntityType = entityCatalog,
                };

                return await _repository.Add(entityFile);
            }

            return Guid.Empty;
        }
    }

    public class UpdateFileService : UpdateModelService<FileDto>
    {
        private IFileManager _fileManager;

        public UpdateFileService(IRepository repository, 
                                 IFileManager fileManager) : base(repository)
        {
            _fileManager = fileManager;
        }

        protected override async Task<bool> Update(FileDto dto)
        {
            Models.Sub.File file = await _repository.GetById<Models.Sub.File>(dto.Id);
            
            file.ViewNameWithExtencion = dto.FileNameWithExtencion;
            string extencion = Path.GetExtension(dto.FileNameWithExtencion);

            await _fileManager.RemoveFile(file.FullFilePath);

            string newFulSavePath = Path.Combine(file.EntityType, dto.Catalog, $"{Guid.NewGuid()}{extencion}");

            if (await _fileManager.SaveFile(newFulSavePath, dto.File)) 
            {
                file.FullFilePath = newFulSavePath;
                return await _repository.Update(file);
            }

            return false;
        }
    }
}
