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
    public class GetIdUserService : IRequestHandler<GetId<UserDto>, IServiceResult<object>>
    {
        private IRepository _repository;
        public GetIdUserService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(GetId<UserDto> request, CancellationToken cancellationToken)
        {
            User logist = await _repository.GetById<User>(request.Id);

            if (logist == null) 
            {
                return new MediatorServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Пользователь не найден"
                };
            }

            UserDto dto = new UserDto()
            {
                Id = logist.Id,
                Name = logist.Name,
                Login = logist.Login,
                IsExpired = logist.IsExpired,
                PasswordState = (PasswordState)logist.PasswordState,
                Role = (UserRole)Enum.Parse(typeof(UserRole), logist.Role),
            };

            return new MediatorServiceResult()
            {
                IsSuccess = true,
                Result = dto
            };
        }
    }

    public class GetRangeUserService : IRequestHandler<GetRange<UserDto>, IServiceResult<object>>
    {
        private IRepository _repository;
        public GetRangeUserService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(GetRange<UserDto> request, CancellationToken cancellationToken)
        {
            IEnumerable<User> logists = await _repository.GetRange<User>(request.Start, request.End, q => q.OrderBy(t => t.Name));
            List<UserDto> dtos = new List<UserDto>();

            foreach (var logist in logists)
            {
                UserDto dto = new UserDto()
                {
                    Id = logist.Id,
                    Name = logist.Name,
                    Login = logist.Login,
                    IsExpired = logist.IsExpired,
                    PasswordState = (PasswordState)logist.PasswordState,
                    Role = (UserRole)Enum.Parse(typeof(UserRole), logist.Role),
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

    public class GetFilterUserService : IRequestHandler<GetFilter<UserDto>, IServiceResult<object>>
    {
        protected IRepository _repository;
        protected ILogger<GetFilterUserService> _logger;

        public GetFilterUserService(IRepository repository, ILogger<GetFilterUserService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IServiceResult<object>> Handle(GetFilter<UserDto> request, CancellationToken cancellationToken)
        {
            Expression<Func<User, bool>> filter = GetFilter(request.PropertyName, request.Params);

            IEnumerable<User> logists = await _repository.Get<User>(filter, q => q.OrderBy(t => t.Name));
            List<UserDto> dtos = new List<UserDto>();

            foreach (var logist in logists)
            {
                UserDto dto = new UserDto()
                {
                    Id = logist.Id,
                    Name = logist.Name,
                    Login = logist.Login,
                    IsExpired = logist.IsExpired,
                    PasswordState = (PasswordState)logist.PasswordState,
                    Role = (UserRole)Enum.Parse(typeof(UserRole), logist.Role),
                };


                dtos.Add(dto);
            }

            return new MediatorServiceResult()
            {
                IsSuccess = true,
                Result = dtos
            };
        }


        protected Expression<Func<User, bool>> GetFilter(string property, params object[] parameters)
        {
            Expression<Func<User, bool>> filter = null;

            try
            {
                switch (property)
                {
                    case nameof(UserDto.Name):
                        string carname = parameters[0].ToString().ToLower();
                        filter = d => d.Name.ToLower().Contains(carname);
                        break;
                    case nameof(UserDto.Login):
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


    public class AddUserService : IRequestHandler<Add<UserDto>, IServiceResult<object>>
    {
        protected IRepository _repository;
        public AddUserService(IRepository repository) 
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(Add<UserDto> request, CancellationToken cancellationToken)
        {
            UserDto dto = request.Value;

            User truck = new User()
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
                    ErrorMessage = "Не удалось сохранить пользователя",
                    Result = Guid.Empty
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

    public class UpdateUserService : IRequestHandler<Update<UserDto>, IServiceResult<object>>
    {
        protected IRepository _repository;
        public UpdateUserService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IServiceResult<object>> Handle(Update<UserDto> request, CancellationToken cancellationToken)
        {
            UserDto dto = request.Value;

            User logist = await _repository.GetById<User>(dto.Id);

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
                    ErrorMessage = "Не удалось обновить пользователя",
                    Result = false
                };
            }
        }
    }

    public class UpdateUserPropertyService : IRequestHandler<Patch<UserDto>, IServiceResult<object>>
    {
        protected IRepository _repository;
        protected ILogger<UpdateUserPropertyService> _logger;

        public UpdateUserPropertyService(IRepository repository, ILogger<UpdateUserPropertyService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IServiceResult<object>> Handle(Patch<UserDto> request, CancellationToken cancellationToken)
        {
            try
            {
                User logist = await _repository.GetById<User>(request.Id);

                foreach (var pair in request.Updates)
                {
                    switch (pair.Key)
                    {
                        case nameof(UserDto.IsExpired):
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
                ErrorMessage = "Не удалось обновить пользователя",
                Result = false
            };
        }
    }

    public class ValidateUserService : IRequestHandler<Validate, IServiceResult<object>>
    {
        private ITokenService<UserDto> _tokenService;
        private IRepository _repository;
        private IHashService _hashService;

        public ValidateUserService(ITokenService<UserDto> tokenService, 
                                     IRepository repository,
                                     IHashService hashService)
        {
            _repository = repository;
            _tokenService = tokenService;
            _hashService = hashService;
        }

        public async Task<IServiceResult<object>> Handle(Validate request, CancellationToken cancellationToken)
        {
            IEnumerable<User> admins = await _repository.Get<User>(l => l.Role == UserRole.Admin.ToString());

            if (!admins.Any()) 
            {
                User logist = new User()
                {
                    Login = "admin",
                    Name = "Админ",
                    Password = _hashService.GetHash("admin"),
                    PasswordState= (short)PasswordState.OnReset,
                    Role = UserRole.Admin.ToString(),
                }; 

                await _repository.Add(logist);
            }

            IEnumerable<User> logists = await _repository.Get<User>(l => l.Login ==  request.Logist.Login && l.Password == request.Logist.Password);

            if (logists.Any())
            {
                User logist = logists.FirstOrDefault();

                if (logist.IsExpired) 
                {
                    return new MediatorServiceResult()
                    {
                        IsSuccess = false,
                        ErrorMessage = "Доступ запрещен",
                    };
                }

                UserDto dto = new UserDto()
                {
                    Id = logist.Id,
                    Name = logist.Name,
                    Login = logist.Login,
                    IsExpired = logist.IsExpired,
                    PasswordState = (PasswordState)logist.PasswordState,
                    Role = (UserRole)Enum.Parse(typeof(UserRole), logist.Role),
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
