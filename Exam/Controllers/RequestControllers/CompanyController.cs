using DTOs;
using MediatR;
using MediatRepos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Utilities.Interfaces;

namespace Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private IQueueService<IRequest<IServiceResult<object>>> _queue;
        private ILogger<CompanyController> _logger;

        public CompanyController(IQueueService<IRequest<IServiceResult<object>>> queue,
                                  ILogger<CompanyController> logger)
        {
            _queue = queue;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult> GetClient(Guid id)
        {
            return Ok(await _queue.Enqueue(new GetId<CompanyDto>(id)));
        }

        [HttpGet("filter/{property}")]
        public virtual async Task<ActionResult> GetClientFilter(string property, [FromQuery] string[] param)
        {
            return Ok(await _queue.Enqueue(new GetFilter<CompanyDto>(property, param)));
        }

        [HttpGet("range/{start}/{end}")]
        public virtual async Task<ActionResult> GetClientRange(int start, int end)
        {
            return Ok(await _queue.Enqueue(new GetRange<CompanyDto>(start, end)));
        }

        [HttpPost()]
        public virtual async Task<ActionResult> PostClient([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _queue.Enqueue(new Add<CompanyDto>(jobj.ToObject<CompanyDto>())));
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
        public virtual async Task<ActionResult> PutClient([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _queue.Enqueue(new Update<CompanyDto>(jobj.ToObject<CompanyDto>())));
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
        public virtual async Task<ActionResult> DeleteClient(Guid id)
        {
            return Ok(await _queue.Enqueue(new Delete<CompanyDto>(id)));
        }
    }
}
