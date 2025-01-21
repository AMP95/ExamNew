using DTOs.Dtos;
using MediatorServices.Abstract;
using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;

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

                if (!_fileManager.SaveFile(savePath, dto.File))
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
                if (_fileManager.RemoveFile(filePath)) 
                { 
                    _fileManager.SaveFile(filePath, dto.File);
                    return true;
                }
            }

            return false;
        }
    }
}
