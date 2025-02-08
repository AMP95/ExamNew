using DTOs;
using MediatR;
using MediatRepos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Utilities.Interfaces;

namespace Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private IQueueService<IRequest<IServiceResult<object>>> _queue;
        private ILogger<DriverController> _logger;

        public DriverController(IQueueService<IRequest<IServiceResult<object>>> queue,
                                  ILogger<DriverController> logger)
        {
            _queue = queue;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult> GetDriver(Guid id)
        {
            return Ok(await _queue.Enqueue(new GetId<DriverDto>(id)));
        }

        [HttpGet("filter/{property}")]
        public virtual async Task<ActionResult> GetDriverFilter(string property, [FromQuery] string[] param)
        {
            return Ok(await _queue.Enqueue(new GetFilter<DriverDto>(property, param)));
        }

        [HttpGet("range/{start}/{end}")]
        public virtual async Task<ActionResult> GetDriverRange(int start, int end)
        {
            return Ok(await _queue.Enqueue(new GetRange<DriverDto>(start, end)));
        }

        [HttpPost()]
        public virtual async Task<ActionResult> PostDriver([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _queue.Enqueue(new Add<DriverDto>(jobj.ToObject<DriverDto>())));
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

        [HttpPut()]
        public virtual async Task<ActionResult> PutDriver([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _queue.Enqueue(new Update<DriverDto>(jobj.ToObject<DriverDto>())));
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

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> DeleteDriver(Guid id)
        {
            return Ok(await _queue.Enqueue(new Delete<DriverDto>(id)));
        }
    }
}
