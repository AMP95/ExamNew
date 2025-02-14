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
    public class TemplateController : ControllerBase
    {
        private IQueueService<IRequest<IServiceResult<object>>> _queue;
        private ILogger<TemplateController> _logger;

        public TemplateController(IQueueService<IRequest<IServiceResult<object>>> queue,
                                  ILogger<TemplateController> logger)
        {
            _queue = queue;
            _logger = logger;
        }
        [HttpGet("{id}")]
        public virtual async Task<ActionResult> GetTemplate(Guid id)
        {
            return Ok(await _queue.Enqueue(new GetId<TemplateDto>(id)));
        }

        [HttpGet("filter/{property}")]
        public virtual async Task<ActionResult> GetTemplateFilter(string property, [FromQuery] string[] param)
        {
            return Ok(await _queue.Enqueue(new GetFilter<TemplateDto>(property, param)));
        }

        [HttpGet("range/{start}/{end}")]
        public virtual async Task<ActionResult> GetTemplateRange(int start, int end)
        {
            return Ok(await _queue.Enqueue(new GetRange<TemplateDto>(start, end)));
        }
        [Authorize]
        [HttpPost()]
        public virtual async Task<ActionResult> PostTemplate([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _queue.Enqueue(new Add<TemplateDto>(jobj.ToObject<TemplateDto>())));
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
        public virtual async Task<ActionResult> PutTemplate([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _queue.Enqueue(new Update<TemplateDto>(jobj.ToObject<TemplateDto>())));
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
        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> DeleteTemplate(Guid id)
        {
            return Ok(await _queue.Enqueue(new Delete<TemplateDto>(id)));
        }
    }
}
