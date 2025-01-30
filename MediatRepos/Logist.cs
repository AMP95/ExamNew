using DTOs;
using DTOs.Dtos;
using MediatorServices.Abstract;
using MediatorServices.Interfaces;
using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.Main;
using System.Linq.Expressions;

namespace MediatorServices
{
    public class GetIdLogistService : GetIdModelService<LogistDto>
    {
        public GetIdLogistService(IRepository repository, ILogger<GetIdModelService<LogistDto>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            Logist logist = await _repository.GetById<Logist>(id);

            LogistDto dto = new LogistDto()
            {
                Id = logist.Id,
                Name = logist.Name,
                Login = logist.Login,
                IsExpired = logist.IsExpired,
                PasswordState = (PasswordState)logist.PasswordState,
                Role = (LogistRole)Enum.Parse(typeof(LogistRole), logist.Role),
            };

            return dto;
        }
    }

    public class GetRangeLogistService : GetRangeModelService<LogistDto>
    {
        public GetRangeLogistService(IRepository repository, ILogger<GetRangeModelService<LogistDto>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(int start, int end)
        {
            IEnumerable<Logist> logists = await _repository.GetRange<Logist>(start, end, q => q.OrderBy(t => t.Name));
            List<LogistDto> dtos = new List<LogistDto>();

            foreach (var logist in logists)
            {
                LogistDto dto = new LogistDto()
                {
                    Id = logist.Id,
                    Name = logist.Name,
                    Login = logist.Login,
                    IsExpired = logist.IsExpired,
                    PasswordState = (PasswordState)logist.PasswordState,
                    Role = (LogistRole)Enum.Parse(typeof(LogistRole), logist.Role),
                };


                dtos.Add(dto);
            }

            return dtos;
        }
    }

    public class GetFilterLogistService : IRequestHandler<GetFilter<LogistDto>, object>
    {
        protected IRepository _repository;
        protected ILogger<GetFilterLogistService> _logger;

        public GetFilterLogistService(IRepository repository, ILogger<GetFilterLogistService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<object> Handle(GetFilter<LogistDto> request, CancellationToken cancellationToken)
        {
            Expression<Func<Logist, bool>> filter = GetFilter(request.PropertyName, request.Params);
            return await Get(filter);
        }

        protected async Task<object> Get(Expression<Func<Logist, bool>> filter)
        {
            IEnumerable<Logist> logists = await _repository.Get<Logist>(filter, q => q.OrderBy(t => t.Name));
            List<LogistDto> dtos = new List<LogistDto>();

            foreach (var logist in logists)
            {
                LogistDto dto = new LogistDto()
                {
                    Id = logist.Id,
                    Name = logist.Name,
                    Login = logist.Login,
                    IsExpired = logist.IsExpired,
                    PasswordState = (PasswordState)logist.PasswordState,
                    Role = (LogistRole)Enum.Parse(typeof(LogistRole), logist.Role),
                };


                dtos.Add(dto);
            }

            return dtos;
        }

        protected Expression<Func<Logist, bool>> GetFilter(string property, params object[] parameters)
        {
            Expression<Func<Logist, bool>> filter = null;

            try
            {
                switch (property)
                {
                    case nameof(LogistDto.Name):
                        string carname = parameters[0].ToString().ToLower();
                        filter = d => d.Name.ToLower().Contains(carname);
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


    public class AddLogistService : AddModelService<LogistDto>
    {
        public AddLogistService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<Guid> Add(LogistDto dto)
        {
            Logist truck = new Logist()
            {
                Name = dto.Name,
                Login = dto.Login,
                Password = dto.Password,
                PasswordState = (short)dto.PasswordState,
                Role = dto.Role.ToString(),
                IsExpired = dto.IsExpired,
            };

            return await _repository.Add(truck);
        }
    }

    public class UpdateLogistService : UpdateModelService<LogistDto>
    {
        public UpdateLogistService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(LogistDto dto)
        {
            Logist vehicle = await _repository.GetById<Logist>(dto.Id);

            vehicle.Name = dto.Name;
            vehicle.Password = dto.Password;
            vehicle.PasswordState = (short)dto.PasswordState;
            vehicle.Role = dto.Role.ToString();
            vehicle.IsExpired = dto.IsExpired;

            return await _repository.Update(vehicle);
        }
    }

    public class ValidateLogistService : IRequestHandler<Validate, object>
    {
        private ITokenService _tokenService;
        private IRepository _repository;
        private IHashService _hashService;

        public ValidateLogistService(ITokenService tokenService, 
                                     IRepository repository,
                                     IHashService hashService)
        {
            _repository = repository;
            _tokenService = tokenService;
            _hashService = hashService;
        }

        public async Task<object> Handle(Validate request, CancellationToken cancellationToken)
        {
            IEnumerable<Logist> admins = await _repository.Get<Logist>(l => l.Role == LogistRole.Admin.ToString());
            if (!admins.Any()) 
            {
                Logist logist = new Logist()
                {
                    Login = "admin",
                    Name = "Админ",
                    Password = _hashService.GetHash("admin"),
                    PasswordState= (short)PasswordState.OnReset,
                    Role = LogistRole.Admin.ToString(),
                }; 

                await _repository.Add(logist);
            }

            IEnumerable<Logist> logists = await _repository.Get<Logist>(l => l.Login ==  request.Logist.Login && l.Password == request.Logist.Password && !l.IsExpired);

            if (logists.Any()) 
            { 
                Logist logist = logists.FirstOrDefault();

                string token = _tokenService.GetToken(logist);

                LogistDto dto = new LogistDto()
                {
                    Id = logist.Id,
                    Name = logist.Name,
                    Login = logist.Login,
                    IsExpired = logist.IsExpired,
                    PasswordState = (PasswordState)logist.PasswordState,
                    Role = (LogistRole)Enum.Parse(typeof(LogistRole), logist.Role),
                };

                return new object[] { token, dto };
            }

            return Array.Empty<object>();
        }
    }
}
