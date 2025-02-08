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
    public class PaymentController : ControllerBase
    {
        private IQueueService<IRequest<IServiceResult<object>>> _queue;
        private ILogger<PaymentController> _logger;

        public PaymentController(IQueueService<IRequest<IServiceResult<object>>> queue,
                                 ILogger<PaymentController> logger)
        {
            _queue = queue;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult> GetPayment(Guid id)
        {
            return Ok(await _queue.Enqueue(new GetId<PaymentDto>(id)));
        }


        [HttpGet("filter/{property}")]
        public virtual async Task<ActionResult> GetPaymentFilter(string property, [FromQuery] string[] param)
        {
            return Ok(await _queue.Enqueue(new GetFilter<PaymentDto>(property, param)));
        }


        [HttpPost()]
        public virtual async Task<ActionResult> PostPayment([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _queue.Enqueue(new Add<PaymentDto>(jobj.ToObject<PaymentDto>())));
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
        public virtual async Task<ActionResult> PutPayment([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _queue.Enqueue(new Update<PaymentDto>(jobj.ToObject<PaymentDto>())));
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
        public virtual async Task<ActionResult> DeletePayment(Guid id)
        {
            return Ok(await _queue.Enqueue(new Delete<PaymentDto>(id)));
        }


    }
}
