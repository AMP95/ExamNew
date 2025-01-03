using Exam.BackgroundServices;
using Exam.ResultServices;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultController : ControllerBase
    {
        private RequestStatusService _statusService;
        public ResultService _resultService;

        public ResultController(RequestStatusService statusService, ResultService resultService)
        {
            _statusService = statusService;
            _resultService = resultService;
        }

        [HttpGet("status/{id}")]
        public async Task<ActionResult> GetStatus(Guid id)
        {
            return Ok(await _statusService.GetStatus(id));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetResult(Guid id)
        {
            ServiceResult result = await _resultService.GetResult(id);
            if (result != null)
            {
                if (result.Result == null)
                {
                    return StatusCode(result.ResultStatusCode, result.ResultErrorMessage);
                }
                else 
                {
                    return StatusCode(result.ResultStatusCode, result.Result);
                }
            }
            return NotFound("Результат не найден");
        }
    }
}

