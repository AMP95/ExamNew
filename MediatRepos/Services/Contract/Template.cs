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
    public class GetIdTemplateService : IRequestHandler<GetId<TemplateDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public GetIdTemplateService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(GetId<TemplateDto> request, CancellationToken cancellationToken)
        {
            IEnumerable<Template> templates = await _repository.Get<Template>(t => t.Id == request.Id, null, "Additionals");

            if (templates.Any())
            {
                Template template = templates.First();

                return new MediatorServiceResult()
                {
                    IsSuccess = true,
                    Result = new TemplateDto()
                    {
                        Id = template.Id,
                        Name = template.Name,
                        Additionals = template.Additionals.Select(x => new AdditionalDto() { Id = x.Id, Name = x.Name, Description = x.Description }).ToList(),
                    }
                };
            }
            else 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Объект не найден"
                };
            }
        }
    }

    public class GetRangeTemplateService : IRequestHandler<GetRange<TemplateDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public GetRangeTemplateService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(GetRange<TemplateDto> request, CancellationToken cancellationToken)
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

            return new MediatorServiceResult()
            {
                IsSuccess = true,
                Result = dtos
            };
        }
    }

    public class GetFilterTemplateService : IRequestHandler<GetFilter<TemplateDto>, IServiceResult<object>>
    {
        protected IRepository _repository;
        protected ILogger<GetFilterTemplateService> _logger;

        public GetFilterTemplateService(IRepository repository, ILogger<GetFilterTemplateService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IServiceResult<object>> Handle(GetFilter<TemplateDto> request, CancellationToken cancellationToken)
        {
            Expression<Func<Template, bool>> filter = GetFilter(request.PropertyName, request.Params);

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

            return new MediatorServiceResult()
            {
                IsSuccess = true,
                Result = dtos
            };
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

    public class DeleteTemplateService : IRequestHandler<Delete<TemplateDto>, IServiceResult<object>>
    {
        private IRepository _repository;
        private IFileManager _fileManager;

        public DeleteTemplateService(IRepository repository, IFileManager fileManager)
        {
            _repository = repository;
            _fileManager = fileManager;
        }

        public async Task<IServiceResult<object>> Handle(Delete<TemplateDto> request, CancellationToken cancellationToken)
        {
            if (await _repository.Remove<Template>(request.Id))
            {
                IEnumerable<Models.Sub.File> files = await _repository.Get<Models.Sub.File>(f => f.EntityType == nameof(Template) && f.EntityId == request.Id);

                if (files.Any())
                {
                    string catalog = Path.GetFileName(Path.GetDirectoryName(files.First().FullFilePath));

                    await _fileManager.RemoveAllFiles(nameof(Template), catalog);
                }

                return new MediatorServiceResult()
                {
                    IsSuccess = true,
                    Result = true,
                };
            }
            else 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Не удалось удалить шаблон"
                };
            }
        }
    }

    public class AddTemplateService : IRequestHandler<Add<TemplateDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public AddTemplateService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(Add<TemplateDto> request, CancellationToken cancellationToken)
        {
            Template template = new Template()
            {
                Name = request.Value.Name,
                Additionals = request.Value.Additionals.Select(ad => new Models.Sub.AdditionalCondition() 
                { 
                    Name = ad.Name, 
                    Description = ad.Description 
                }).ToList()
            };

            Guid id = await _repository.Add(template);

            if (id == Guid.Empty)
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Не удалось добавить шаблон"
                };
            }
            else 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = true,
                    Result = id
                };
            }
        }
    }

    public class UpdateTemplateService : IRequestHandler<Add<TemplateDto>, IServiceResult<object>>
    {
        private IRepository _repository;

        public UpdateTemplateService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(Add<TemplateDto> request, CancellationToken cancellationToken)
        {
            TemplateDto dto = request.Value;

            Template template = await _repository.GetById<Template>(dto.Id);

            template.Name = dto.Name;
            template.Additionals.Clear();
            template.Additionals = dto.Additionals.Select(ad => new Models.Sub.AdditionalCondition() { Name = ad.Name, Description = ad.Description }).ToList();

            if (await _repository.Update(template))
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = true,
                    Result = true,
                };
            }
            else 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Не удалось обновить шаблон"
                };
            }
        }
    }
}
