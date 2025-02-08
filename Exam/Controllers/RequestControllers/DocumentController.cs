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
    public class DocumentController : ControllerBase
    {
        private IQueueService<IRequest<IServiceResult<object>>> _queue;
        private ILogger<DocumentController> _logger;

        public DocumentController(IQueueService<IRequest<IServiceResult<object>>> queue,
                                  ILogger<DocumentController> logger)
        {
            _queue = queue;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult> GetDocument(Guid id)
        {
            return Ok(await _queue.Enqueue(new GetId<DocumentDto>(id)));
        }

        [HttpGet("filter/{property}")]
        public virtual async Task<ActionResult> GetDocumentFilter(string property, [FromQuery] string[] param)
        {
            return Ok(await _queue.Enqueue(new GetFilter<DocumentDto>(property, param)));
        }

        [HttpPost()]
        public virtual async Task<ActionResult> PostDocument([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _queue.Enqueue(new Add<DocumentDto>(jobj.ToObject<DocumentDto>())));
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
        public virtual async Task<ActionResult> PutDocument([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _queue.Enqueue(new Update<DocumentDto>(jobj.ToObject<DocumentDto>())));
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
        public virtual async Task<ActionResult> DeleteDocument(Guid id)
        {
            return Ok(await _queue.Enqueue(new Delete<DocumentDto>(id)));
        }
    }
}
