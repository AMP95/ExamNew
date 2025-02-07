using MediatorServices;
using Microsoft.AspNetCore.Mvc;
using Utilities.Interfaces;

namespace Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultController : ControllerBase
    {
        private IStatusService _statusService;
        private IResultService _resultService;

        public ResultController(IStatusService statusService, IResultService resultService)
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
            IServiceResult<object> result = await _resultService.GetResult(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Результат не найден");
        }

        [HttpGet("file/{id}")]
        public async Task<ActionResult> GetFileResult(Guid id)
        {
            IServiceResult<object> result = await _resultService.GetResult(id);

            if (result != null)
            {
                if (result.IsSuccess)
                {
                    var fileRusult = result.Result as FileSendResult;

                    return File(fileRusult.Data, fileRusult.ContentType, fileRusult.FileName);
                }
                else 
                { 
                    Ok(result);
                }
            }
            return NotFound("Результат не найден");
        }
    }
}

