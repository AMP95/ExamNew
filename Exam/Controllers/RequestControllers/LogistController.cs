using DTOs.Dtos;
using MediatR;
using MediatRepos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Utilities.Interfaces;

namespace Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogistController : ControllerBase
    {
        private IQueueService<IRequest<IServiceResult<object>>> _queue;
        private ILogger<LogistController> _logger;

        public LogistController(IQueueService<IRequest<IServiceResult<object>>> queue,
                                ILogger<LogistController> logger)
        {
            _queue = queue;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult> GetLogist(Guid id)
        {
            return Ok(await _queue.Enqueue(new GetId<LogistDto>(id)));
        }

        [HttpGet("filter/{property}")]
        public virtual async Task<ActionResult> GetLogistFilter(string property, [FromQuery] string[] param)
        {
            return Ok(await _queue.Enqueue(new GetFilter<LogistDto>(property, param)));
        }

        [HttpGet("range/{start}/{end}")]
        public virtual async Task<ActionResult> GetLogistRange(int start, int end)
        {
            return Ok(await _queue.Enqueue(new GetRange<LogistDto>(start, end)));
        }

        [HttpPost()]
        public virtual async Task<ActionResult> PostLogist([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _queue.Enqueue(new Add<LogistDto>(jobj.ToObject<LogistDto>())));
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
                    return Ok(await _queue.Enqueue(new Validate(jobj.ToObject<LogistDto>())));
                }
                _logger.LogWarning($"Recieved null object");
                return BadRequest("Передан пустой параметр");
            }
            catch (Exception ex)
            {
                return BadRequest("Неверный тип данных");
            }
        }

        [HttpPut()]
        public virtual async Task<ActionResult> PutLogist([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _queue.Enqueue(new Update<LogistDto>(jobj.ToObject<LogistDto>())));
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

        [HttpPatch("{id}")]
        public virtual async Task<ActionResult> PatchLogist(Guid id, [FromBody] JArray updates)
        {
            try
            {
                if (updates != null)
                {
                    return Ok(await _queue.Enqueue(new Patch<LogistDto>(id, updates.ToObject<KeyValuePair<string, object>[]>())));
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
