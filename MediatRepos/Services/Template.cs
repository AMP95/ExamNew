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
    public class GetIdTemplateService : GetIdModelService<TemplateDto>
    {
        public GetIdTemplateService(IRepository repository, ILogger<GetIdModelService<TemplateDto>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            IEnumerable<Template> templates = await _repository.Get<Template>(t => t.Id == id, null, "Additionals");

            TemplateDto dto = null;
            if (templates.Any())
            {
                Template template = templates.First();

                dto = new TemplateDto()
                {
                    Id = template.Id,
                    Name = template.Name,
                    Additionals = template.Additionals.Select(x => new AdditionalDto() { Id = x.Id, Name = x.Name, Description = x.Description }).ToList(),
                };
            }
            return dto;
        }
    }

    public class GetRangeTemplateService : GetRangeModelService<TemplateDto>
    {
        public GetRangeTemplateService(IRepository repository, ILogger<GetRangeModelService<TemplateDto>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(int start, int end)
        {
            IEnumerable<Template> templates = await _repository.Get<Template>(null, null, "Additionals");
            List<TemplateDto> dtos = new List<TemplateDto>();

            foreach (var template in templates)
            {
                TemplateDto dto = new TemplateDto()
                {
                    Id = template.Id,
                    Name = template.Name,
                };

                dtos.Add(dto);
            }

            return dtos;
        }
    }

    public class GetFilterTemplateService : IRequestHandler<GetFilter<TemplateDto>, object>
    {
        protected IRepository _repository;
        protected ILogger<GetFilterTemplateService> _logger;

        public GetFilterTemplateService(IRepository repository, ILogger<GetFilterTemplateService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<object> Handle(GetFilter<TemplateDto> request, CancellationToken cancellationToken)
        {
            Expression<Func<Template, bool>> filter = GetFilter(request.PropertyName, request.Params);
            return await Get(filter);
        }

        protected async Task<object> Get(Expression<Func<Template, bool>> filter)
        {
            IEnumerable<Template> templates = await _repository.Get<Template>(filter);
            List<TemplateDto> dtos = new List<TemplateDto>();

            foreach (var template in templates)
            {
                TemplateDto dto = new TemplateDto()
                {
                    Id = template.Id,
                    Name = template.Name,
                };

                dtos.Add(dto);
            }

            return dtos;
        }

        protected Expression<Func<Template, bool>> GetFilter(string property, params object[] parameters)
        {
            Expression<Func<Template, bool>> filter = null;

            try
            {
                switch (property)
                {
                    case nameof(TemplateDto.Name):
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

    public class DeleteTemplateService : IRequestHandler<Delete<TemplateDto>, bool>
    {
        private IRepository _repository;
        private IFileManager _fileManager;

        public DeleteTemplateService(IRepository repository, IFileManager fileManager)
        {
            _repository = repository;
            _fileManager = fileManager;
        }

        public async Task<bool> Handle(Delete<TemplateDto> request, CancellationToken cancellationToken)
        {
            bool result = await _repository.Remove<Template>(request.Id);

            IEnumerable<Models.Sub.File> files = await _repository.Get<Models.Sub.File>(f => f.EntityType == nameof(Template) && f.EntityId == request.Id);

            if (files.Any()) 
            {
                string catalog = Path.GetFileName(Path.GetDirectoryName(files.First().FullFilePath));

                await _fileManager.RemoveAllFiles(nameof(Template), catalog);
            }

            return result;
        }
    }

    public class AddTemplateService : AddModelService<TemplateDto>
    {
        public AddTemplateService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<Guid> Add(TemplateDto dto)
        {
            Template template = new Template()
            {
                Name = dto.Name,
                Additionals = dto.Additionals.Select(ad => new Models.Sub.AdditionalCondition() { Name = ad.Name, Description = ad.Description }).ToList()
            };

            return await _repository.Add(template);
        }
    }

    public class UpdateTemplateService : UpdateModelService<TemplateDto>
    {
        public UpdateTemplateService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(TemplateDto dto)
        {
            Template template = await _repository.GetById<Template>(dto.Id);

            template.Name = dto.Name;
            template.Additionals.Clear();
            template.Additionals = dto.Additionals.Select(ad => new Models.Sub.AdditionalCondition() { Name = ad.Name, Description = ad.Description }).ToList();

            return await _repository.Update(template);
        }
    }
}
