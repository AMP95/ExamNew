using DTOs;
using DTOs.Dtos;
using MediatorServices.Abstract;
using MediatR;
using MediatRepos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
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
                    }
                };

                if (template.File != null)
                {
                    FormFile formFile;
                    using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(template.File.Path).ToArray()))
                    {
                        formFile = new FormFile(stream, 0, stream.Length, "streamFile", template.File.Path.Split(@"\").Last());
                    }

                    dto.File = new FileDto()
                    {
                        Id = template.File.Id,
                        File = formFile
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
        private IFileManager _fileManager;
        public AddTemplateService(IRepository repository, IFileManager fileManager) : base(repository)
        {
            _fileManager = fileManager;
        }

        protected override async Task<bool> Update(ContractTemplateDto dto)
        {
            ContractTemplate template = new ContractTemplate()
            {
                Name = dto.Name,
            };

            if (await _repository.Update(template))
            {

                if (dto.File != null)
                {
                    string filePath = Path.Combine("Files", "Templates", dto.Name);

                    if (_fileManager.Save(filePath, dto.File.File))
                    {
                        template.File = new Models.Sub.File()
                        {
                            Path = filePath,
                            EntityId = template.Id,
                            EntityType = typeof(ContractTemplate)
                        };

                        return await _repository.Update(template);
                    }
                }

                return true;
            }
            else 
            { 
                return false;
            }
        }
    }
}
