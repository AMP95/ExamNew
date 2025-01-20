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
            IEnumerable<ContractTemplate> templates = await _repository.Get<ContractTemplate>(t => t.Id == id, null, "File");
            ContractTemplateDto dto = null;
            if (templates.Any())
            {
                ContractTemplate template = templates.First();
                dto = new ContractTemplateDto()
                {
                    Id = template.Id,
                    Name = template.Name,
                    File = new FileDto() 
                    {
                        Id = template.File.Id,
                        Name = template.File.Name,
                    }
                };
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
            IEnumerable<ContractTemplate> templates = await _repository.Get<ContractTemplate>(null,null, "File");
            List<ContractTemplateDto> dtos = new List<ContractTemplateDto>();

            foreach (var template in templates)
            {
                ContractTemplateDto dto = new ContractTemplateDto()
                {
                    Id = template.Id,
                    Name = template.Name,
                };

                if (template.File != null) 
                {
                    dto.File = new FileDto()
                    {
                        Id = template.File.Id,
                    };
                }

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
                FileId = dto.File.Id
            };

            return await _repository.Add(template);
        }
    }
}
