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
    public class VehicleController : ControllerBase
    {
        private IQueueService<IRequest<IServiceResult<object>>> _queue;
        private ILogger<VehicleController> _logger;

        public VehicleController(IQueueService<IRequest<IServiceResult<object>>> queue,
                                  ILogger<VehicleController> logger)
        {
            _queue = queue;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult> GetVehicle(Guid id)
        {
            return Ok(await _queue.Enqueue(new GetId<VehicleDto>(id)));
        }

        [HttpGet("filter/{property}")]
        public virtual async Task<ActionResult> GetVehicleFilter(string property, [FromQuery] string[] param)
        {
            return Ok(await _queue.Enqueue(new GetFilter<VehicleDto>(property, param)));
        }


        [HttpGet("range/{start}/{end}")]
        public virtual async Task<ActionResult> GetVehicleRange(int start, int end)
        {
            return Ok(await _queue.Enqueue(new GetRange<VehicleDto>(start, end)));
        }

        [HttpPost()]
        public virtual async Task<ActionResult> PostVehicle([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _queue.Enqueue(new Add<VehicleDto>(jobj.ToObject<VehicleDto>())));
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
        public virtual async Task<ActionResult> PutVehicle([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _queue.Enqueue(new Update<VehicleDto>(jobj.ToObject<VehicleDto>())));
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
        public virtual async Task<ActionResult> DeleteVehicle(Guid id)
        {
            return Ok(await _queue.Enqueue(new Delete<VehicleDto>(id)));
        }
    }
}
