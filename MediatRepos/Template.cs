using DTOs.Dtos;
using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.Main;

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
                IEnumerable<Models.Sub.File> files = await _repository.Get<Models.Sub.File>(f => f.EntityId == template.Id);

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
                        FileName = $"{file.Name}{file.Extencion}",
                        EntityId = file.EntityId,
                        EntityType = file.EntityType,
                        SubFolder = file.Subfolder
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

    public class DeleteTemplateService : IRequestHandler<Delete<ContractTemplateDto>, bool>
    {
        private IRepository _repository;

        public DeleteTemplateService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(Delete<ContractTemplateDto> request, CancellationToken cancellationToken)
        {
            return await _repository.Remove<ContractTemplate>(request.Id);
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
