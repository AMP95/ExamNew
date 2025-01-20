using DTOs.Dtos;
using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;

namespace MediatorServices
{
    public class GetIdFileService : GetIdModelService<FileDto>
    {
        public GetIdFileService(IRepository repository, ILogger<GetIdModelService<FileDto>> logger) : base(repository, logger)
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
                    Name = file.Name,
                    SaveName = file.SaveName,
                };
            }
            return null;
        }
    }

    public class DeleteFileService : IRequestHandler<Delete<FileDto>, bool>
    {
        private IRepository _repository;

        public DeleteFileService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(Delete<FileDto> request, CancellationToken cancellationToken)
        {
            return await _repository.Remove<Models.Sub.File>(request.Id);
        }
    }

    public class AddFileService : AddModelService<FileDto>
    {
        public AddFileService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<Guid> Add(FileDto dto)
        {
            Models.Sub.File file = new Models.Sub.File()
            {
                Name = dto.Name,
                SaveName = dto.SaveName,
            };

            return await _repository.Add(file);
        }
    }

    public class UpdateFileService : UpdateModelService<FileDto>
    {
        public UpdateFileService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(FileDto dto)
        {
            Models.Sub.File file = await _repository.GetById<Models.Sub.File>(dto.Id);

            if (file != null) 
            { 
                file.Name = dto.Name;
                file.SaveName = dto.SaveName;

                return await _repository.Update(file);
            }

            return false;
        }
    }
}
