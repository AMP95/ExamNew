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
                FileDto dto = new FileDto()
                {
                    Id = id,
                    FileName = file.Name,
                    SubFolder = file.Subfolder,
                    EntityId = file.EntityId,
                };

                switch (file.EntityType) 
                {
                    case nameof(Carrier):
                        dto.EntityType = typeof(CarrierDto);
                        break;
                    case nameof(Driver):
                        dto.EntityType = typeof(DriverDto);
                        break;
                    case nameof(Vehicle):
                        dto.EntityType = typeof(VehicleDto);
                        break;
                    case nameof(Contract):
                        dto.EntityType = typeof(ContractDto);
                        break;
                    case nameof(ContractTemplate):
                        dto.EntityType = typeof(ContractTemplateDto);
                        break;
                }
                return dto;
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
                FileDto dto = new FileDto()
                {
                    Id = file.Id,
                    FileName = $"{file.Name}{file.Extencion}",
                    SubFolder = file.Subfolder,
                    EntityId = file.EntityId,
                };

                switch (file.EntityType)
                {
                    case nameof(Carrier):
                        dto.EntityType = typeof(CarrierDto);
                        break;
                    case nameof(Driver):
                        dto.EntityType = typeof(DriverDto);
                        break;
                    case nameof(Vehicle):
                        dto.EntityType = typeof(VehicleDto);
                        break;
                    case nameof(Contract):
                        dto.EntityType = typeof(ContractDto);
                        break;
                    case nameof(ContractTemplate):
                        dto.EntityType = typeof(ContractTemplateDto);
                        break;
                }

                dtos.Add(dto);
            }

            return dtos;
        }

        protected Expression<Func<Models.Sub.File, bool>> GetFilter(string property, params object[] parameters)
        {
            Expression<Func<Models.Sub.File, bool>> filter = null;

            try
            {
                if (property == nameof(FileDto.EntityId) && Guid.TryParse(parameters[0].ToString(), out var entityId)) 
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
                string path = Path.Combine(file.Subfolder, $"{file.Id}{file.Extencion}");
                if (await _repository.Remove<Models.Sub.File>(request.Id))
                {
                    _fileManager.RemoveFile(path);
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
            string extencion = Path.GetExtension(dto.FileName);
            string name = Path.GetFileNameWithoutExtension(dto.FileName);

            Models.Sub.File file = new Models.Sub.File()
            {
                Name = name,
                Extencion = extencion,
                Subfolder = dto.SubFolder,
                EntityId = dto.EntityId,
            };

            switch (dto.EntityType.Name) 
            {
                case nameof(CarrierDto):
                    file.EntityType = nameof(Carrier);
                    break;
                case nameof(DriverDto):
                    file.EntityType = nameof(Driver);
                    break;
                case nameof(VehicleDto):
                    file.EntityType = nameof(Vehicle);
                    break;
                case nameof(ContractDto):
                    file.EntityType = nameof(Contract);
                    break;
                case nameof(ContractTemplateDto):
                    file.EntityType = nameof(ContractTemplate);
                    break;

            }

            Guid id = await _repository.Add(file);

            if (id != Guid.Empty) 
            {
                string saveDirectory = Path.Combine(file.EntityType, file.Subfolder);

                if (!await _fileManager.SaveFile(saveDirectory, $"{file.Name}{file.Extencion}", dto.File))
                {
                    await _repository.Remove<Models.Sub.File>(id);
                    id = Guid.Empty;
                }
            }

            return id;
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
            string extencion = Path.GetExtension(dto.FileName);
            string name = Path.GetFileNameWithoutExtension(dto.FileName);

            Models.Sub.File file = await _repository.GetById<Models.Sub.File>(dto.Id);

            file.Name = name;
            file.Extencion = extencion;

            if (await _repository.Update(file)) 
            {
                string fileDirectory = Path.Combine(file.EntityType, file.Subfolder);
                string filePath = Path.Combine(fileDirectory, $"{file.Id}{file.Extencion}");
                if (await _fileManager.RemoveFile(filePath)) 
                { 
                    _fileManager.SaveFile(fileDirectory, $"{file.Id}{file.Extencion}", dto.File);
                    return true;
                }
            }

            return false;
        }
    }
}
