using DTOs;
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
    public class GetIdLogistService : IRequestHandler<GetId<LogistDto>, IServiceResult<object>>
    {
        private IRepository _repository;
        public GetIdLogistService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(GetId<LogistDto> request, CancellationToken cancellationToken)
        {
            Logist logist = await _repository.GetById<Logist>(request.Id);

            if (logist == null) 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Пользователь не найден"
                };
            }

            LogistDto dto = new LogistDto()
            {
                Id = logist.Id,
                Name = logist.Name,
                Login = logist.Login,
                IsExpired = logist.IsExpired,
                PasswordState = (PasswordState)logist.PasswordState,
                Role = (LogistRole)Enum.Parse(typeof(LogistRole), logist.Role),
            };

            return new MediatorServiceResult()
            {
                IsSuccess = true,
                Result = dto
            };
        }
    }

    public class GetRangeLogistService : IRequestHandler<GetRange<LogistDto>, IServiceResult<object>>
    {
        private IRepository _repository;
        public GetRangeLogistService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(GetRange<LogistDto> request, CancellationToken cancellationToken)
        {
            IEnumerable<Logist> logists = await _repository.GetRange<Logist>(request.Start, request.End, q => q.OrderBy(t => t.Name));
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

            return new MediatorServiceResult()
            {
                IsSuccess = true,
                Result = dtos
            };
        }
    }

    public class GetFilterLogistService : IRequestHandler<GetFilter<LogistDto>, IServiceResult<object>>
    {
        protected IRepository _repository;
        protected ILogger<GetFilterLogistService> _logger;

        public GetFilterLogistService(IRepository repository, ILogger<GetFilterLogistService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IServiceResult<object>> Handle(GetFilter<LogistDto> request, CancellationToken cancellationToken)
        {
            Expression<Func<Logist, bool>> filter = GetFilter(request.PropertyName, request.Params);

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

            return new MediatorServiceResult()
            {
                IsSuccess = true,
                Result = dtos
            };
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
                    case nameof(LogistDto.Login):
                        string login = parameters[0].ToString().ToLower();
                        filter = d => d.Login.ToLower().Contains(login);
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


    public class AddLogistService : IRequestHandler<Update<LogistDto>, IServiceResult<object>>
    {
        protected IRepository _repository;
        public AddLogistService(IRepository repository) 
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(Update<LogistDto> request, CancellationToken cancellationToken)
        {
            LogistDto dto = request.Value;

            Logist truck = new Logist()
            {
                Name = dto.Name,
                Login = dto.Login,
                Password = dto.Password,
                PasswordState = (short)dto.PasswordState,
                Role = dto.Role.ToString(),
                IsExpired = dto.IsExpired,
            };

            Guid id = await _repository.Add(truck);

            if (id == Guid.Empty)
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Не удалось сохранить пользователя"
                };
            }
            else 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = true,
                    Result = id,
                };
            }
        }
    }

    public class UpdateLogistService : IRequestHandler<Update<LogistDto>, IServiceResult<object>>
    {
        protected IRepository _repository;
        public UpdateLogistService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(Update<LogistDto> request, CancellationToken cancellationToken)
        {
            LogistDto dto = request.Value;

            Logist logist = await _repository.GetById<Logist>(dto.Id);

            logist.Name = dto.Name;
            logist.Password = dto.Password;
            logist.PasswordState = (short)dto.PasswordState;
            logist.Role = dto.Role.ToString();
            logist.IsExpired = dto.IsExpired;

            if (await _repository.Update(logist))
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
                    ErrorMessage = "Не удалось обновить пользователя"
                };
            }
        }
    }

    public class UpdateLogistPropertyService : IRequestHandler<Patch<LogistDto>, IServiceResult<object>>
    {
        protected IRepository _repository;
        protected ILogger<UpdateLogistPropertyService> _logger;

        public UpdateLogistPropertyService(IRepository repository, ILogger<UpdateLogistPropertyService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IServiceResult<object>> Handle(Patch<LogistDto> request, CancellationToken cancellationToken)
        {
            try
            {
                Logist logist = await _repository.GetById<Logist>(request.Id);

                foreach (var pair in request.Updates)
                {
                    switch (pair.Key)
                    {
                        case nameof(LogistDto.IsExpired):
                            bool expiration = (bool)pair.Value;
                            logist.IsExpired = expiration;
                            break;
                    }
                }

                if (await _repository.Update(logist)) 
                {
                    return new MediatorServiceResult()
                    {
                        IsSuccess = true,
                        Result = true,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return new MediatorServiceResult()
            {
                IsSuccess = false,
                ErrorMessage = "Не удалось обновить пользователя"
            };
        }
    }

    public class ValidateLogistService : IRequestHandler<Validate, IServiceResult<object>>
    {
        private ITokenService<LogistDto> _tokenService;
        private IRepository _repository;
        private IHashService _hashService;

        public ValidateLogistService(ITokenService<LogistDto> tokenService, 
                                     IRepository repository,
                                     IHashService hashService)
        {
            _repository = repository;
            _tokenService = tokenService;
            _hashService = hashService;
        }

        public async Task<IServiceResult<object>> Handle(Validate request, CancellationToken cancellationToken)
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

            IEnumerable<Logist> logists = await _repository.Get<Logist>(l => l.Login ==  request.Logist.Login && l.Password == request.Logist.Password);

            if (logists.Any())
            {
                Logist logist = logists.FirstOrDefault();

                if (logist.IsExpired) 
                {
                    return new MediatorServiceResult()
                    {
                        IsSuccess = false,
                        ErrorMessage = "Доступ запрещен"
                    };
                }

                LogistDto dto = new LogistDto()
                {
                    Id = logist.Id,
                    Name = logist.Name,
                    Login = logist.Login,
                    IsExpired = logist.IsExpired,
                    PasswordState = (PasswordState)logist.PasswordState,
                    Role = (LogistRole)Enum.Parse(typeof(LogistRole), logist.Role),
                };

                string token = _tokenService.GetToken(dto);

                return new MediatorServiceResult()
                {
                    IsSuccess = true,
                    Result = new object[] { token, dto }
                };
            }
            else 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Неверный логин или пароль"
                };
            }
        }
    }
}
