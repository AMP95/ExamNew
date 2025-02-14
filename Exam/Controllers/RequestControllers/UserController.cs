using DTOs.Dtos;
using MediatR;
using MediatRepos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Utilities.Interfaces;

namespace Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IQueueService<IRequest<IServiceResult<object>>> _queue;
        private ILogger<UserController> _logger;

        public UserController(IQueueService<IRequest<IServiceResult<object>>> queue,
                                ILogger<UserController> logger)
        {
            _queue = queue;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult> GetLogist(Guid id)
        {
            return Ok(await _queue.Enqueue(new GetId<UserDto>(id)));
        }

        [HttpGet("filter/{property}")]
        public virtual async Task<ActionResult> GetLogistFilter(string property, [FromQuery] string[] param)
        {
            return Ok(await _queue.Enqueue(new GetFilter<UserDto>(property, param)));
        }

        [HttpGet("range/{start}/{end}")]
        public virtual async Task<ActionResult> GetLogistRange(int start, int end)
        {
            return Ok(await _queue.Enqueue(new GetRange<UserDto>(start, end)));
        }
        [Authorize]
        [HttpPost()]
        public virtual async Task<ActionResult> PostLogist([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _queue.Enqueue(new Add<UserDto>(jobj.ToObject<UserDto>())));
                }
                _logger.LogWarning($"Recieved null object");
                return BadRequest("Передан пустой параметр");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("Неверный тип данных");
            }
        }

        [HttpPost("validate")]
        public virtual async Task<ActionResult> ValidateLogist([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _queue.Enqueue(new Validate(jobj.ToObject<UserDto>())));
                }
                _logger.LogWarning($"Recieved null object");
                return BadRequest("Передан пустой параметр");
            }
            catch (Exception ex)
            {
                return BadRequest("Неверный тип данных");
            }
        }
        [Authorize]
        [HttpPut()]
        public virtual async Task<ActionResult> PutLogist([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _queue.Enqueue(new Update<UserDto>(jobj.ToObject<UserDto>())));
                }
                _logger.LogWarning($"Recieved null object");
                return BadRequest("Передан пустой параметр");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("Неверный тип данных");
            }

        }
        [Authorize]
        [HttpPatch("{id}")]
        public virtual async Task<ActionResult> PatchLogist(Guid id, [FromBody] JArray updates)
        {
            try
            {
                if (updates != null)
                {
                    return Ok(await _queue.Enqueue(new Patch<UserDto>(id, updates.ToObject<KeyValuePair<string, object>[]>())));
                }
                _logger.LogWarning($"Recieved null object");
                return BadRequest("Передан пустой параметр");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("Передан неверный тип данных");
            }

        }

        
    }
}
