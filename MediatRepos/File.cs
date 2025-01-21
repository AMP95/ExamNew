using DTOs.Dtos;
using MediatorServices.Abstract;
using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
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
                return new FileDto()
                {
                    Id = id,
                    FileName = file.Name,
                    SubFolder = file.Subfolder,
                    EntityId = file.EntityId,
                    EntityType = file.EntityType,
                };
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
                    EntityType = file.EntityType,
                };

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
                EntityType = dto.EntityType,
            };

            Guid id = await _repository.Add(file);

            if (id != Guid.Empty) 
            {
                string savePath = Path.Combine(file.Subfolder, $"{file.Id}{file.Extencion}");

                if (!await _fileManager.SaveFile(savePath, dto.File))
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
                string filePath = Path.Combine(file.Subfolder, $"{file.Id}{file.Extencion}");
                if (await _fileManager.RemoveFile(filePath)) 
                { 
                    _fileManager.SaveFile(filePath, dto.File);
                    return true;
                }
            }

            return false;
        }
    }
}
