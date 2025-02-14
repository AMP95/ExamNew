using DTOs;
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
    public class ContractController : ControllerBase
    {
        private IQueueService<IRequest<IServiceResult<object>>> _queue;
        private ILogger<ContractController> _logger;

        public ContractController(IQueueService<IRequest<IServiceResult<object>>> queue,
                                  ILogger<ContractController> logger)
        {
            _queue = queue;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult> GetContract(Guid id)
        {
            return Ok(await _queue.Enqueue(new GetId<ContractDto>(id)));
        }

        [HttpGet("filter/{property}")]
        public virtual async Task<ActionResult> GetContractFilter(string property, [FromQuery] string[] param)
        {
            return Ok(await _queue.Enqueue(new GetFilter<ContractDto>(property, param)));
        }

        [HttpGet("payment")]
        public virtual async Task<ActionResult> GetContractPayment()
        {
            return Ok(await _queue.Enqueue(new GetRequiredToPay()));
        }
        [Authorize]
        [HttpPost()]
        public virtual async Task<ActionResult> PostContract([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _queue.Enqueue(new Add<ContractDto>(jobj.ToObject<ContractDto>())));
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
        [HttpPut()]
        public virtual async Task<ActionResult> PutContract([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _queue.Enqueue(new Update<ContractDto>(jobj.ToObject<ContractDto>())));
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
        public virtual async Task<ActionResult> PatchContract(Guid id, [FromBody] JArray updates)
        {
            try
            {
                if (updates != null)
                {
                    return Ok(await _queue.Enqueue(new Patch<ContractDto>(id, updates.ToObject<KeyValuePair<string, object>[]>())));
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
