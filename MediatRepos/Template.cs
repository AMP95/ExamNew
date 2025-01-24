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
    public class GetIdTemplateService : GetIdModelService<ContractTemplateDto>
    {
        public GetIdTemplateService(IRepository repository, ILogger<GetIdModelService<ContractTemplateDto>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            IEnumerable<ContractTemplate> templates = await _repository.Get<ContractTemplate>(t => t.Id == id);

            ContractTemplateDto dto = null;
            if (templates.Any())
            {
                ContractTemplate template = templates.First();
                IEnumerable<Models.Sub.File> files = await _repository.Get<Models.Sub.File>(f => f.EntityType == nameof(ContractTemplate) && f.EntityId == template.Id);

                dto = new ContractTemplateDto()
                {
                    Id = template.Id,
                    Name = template.Name,
                };

                if (files.Any()) 
                {
                    Models.Sub.File file = files.First();

                    dto.File = new FileDto()
                    {
                        Id = file.Id,
                        FileNameWithExtencion = file.ViewNameWithExtencion,
                        Catalog = Path.GetFileName(Path.GetDirectoryName(file.FullFilePath)),
                        DtoId = file.EntityId,
                        DtoType = file.EntityType
                    };
                }
            }
            return dto;
        }
    }

    public class GetRangeTemplateService : GetRangeModelService<ContractTemplateDto>
    {
        public GetRangeTemplateService(IRepository repository, ILogger<GetRangeModelService<ContractTemplateDto>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(int start, int end)
        {
            IEnumerable<ContractTemplate> templates = await _repository.Get<ContractTemplate>();
            List<ContractTemplateDto> dtos = new List<ContractTemplateDto>();

            foreach (var template in templates)
            {
                ContractTemplateDto dto = new ContractTemplateDto()
                {
                    Id = template.Id,
                    Name = template.Name,
                };

                dtos.Add(dto);
            }

            return dtos;
        }
    }

    public class GetFilterTemplateService : IRequestHandler<GetFilter<ContractTemplateDto>, object>
    {
        protected IRepository _repository;
        protected ILogger<GetFilterTemplateService> _logger;

        public GetFilterTemplateService(IRepository repository, ILogger<GetFilterTemplateService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<object> Handle(GetFilter<ContractTemplateDto> request, CancellationToken cancellationToken)
        {
            Expression<Func<ContractTemplate, bool>> filter = GetFilter(request.PropertyName, request.Params);
            return await Get(filter);
        }

        protected async Task<object> Get(Expression<Func<ContractTemplate, bool>> filter)
        {
            IEnumerable<ContractTemplate> templates = await _repository.Get<ContractTemplate>(filter);
            List<ContractTemplateDto> dtos = new List<ContractTemplateDto>();

            foreach (var template in templates)
            {
                ContractTemplateDto dto = new ContractTemplateDto()
                {
                    Id = template.Id,
                    Name = template.Name,
                };

                dtos.Add(dto);
            }

            return dtos;
        }

        protected Expression<Func<ContractTemplate, bool>> GetFilter(string property, params object[] parameters)
        {
            Expression<Func<ContractTemplate, bool>> filter = null;

            try
            {
                switch (property)
                {
                    case nameof(ContractTemplateDto.Name):
                        string name = parameters[0].ToString().ToLower();
                        filter = d => d.Name.ToLower().Contains(name);
                        break;
                }
            }
            catch (Exception ex)
            {
                filter = v => false;
                _logger.LogError(ex, ex.Message);
            }

            return filter;
        }
    }

    public class DeleteTemplateService : IRequestHandler<Delete<ContractTemplateDto>, bool>
    {
        private IRepository _repository;
        private IFileManager _fileManager;

        public DeleteTemplateService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(Delete<ContractTemplateDto> request, CancellationToken cancellationToken)
        {
            bool result = await _repository.Remove<ContractTemplate>(request.Id);

            IEnumerable<Models.Sub.File> files = await _repository.Get<Models.Sub.File>(f => f.EntityType == nameof(ContractTemplate) && f.EntityId == request.Id);

            if (files.Any()) 
            {
                string catalog = Path.GetFileName(Path.GetDirectoryName(files.First().FullFilePath));

                await _fileManager.RemoveAllFiles(nameof(ContractTemplate), catalog);
            }

            return result;
        }
    }

    public class AddTemplateService : AddModelService<ContractTemplateDto>
    {
        public AddTemplateService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<Guid> Add(ContractTemplateDto dto)
        {
            ContractTemplate template = new ContractTemplate()
            {
                Name = dto.Name,
            };

            return await _repository.Add(template);
        }
    }
}
