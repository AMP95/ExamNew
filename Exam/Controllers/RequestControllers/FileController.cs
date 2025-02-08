using DTOs.Dtos;
using MediatR;
using MediatRepos;
using Microsoft.AspNetCore.Mvc;
using Utilities.Interfaces;

namespace Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private IQueueService<IRequest<IServiceResult<object>>> _queue;
        private ILogger<FileController> _logger;
        private IFileManager _fileManager;

        public FileController(IQueueService<IRequest<IServiceResult<object>>> queue,
                              ILogger<FileController> logger,
                              IFileManager fileManager)
        {
            _queue = queue;
            _logger = logger;
            _fileManager = fileManager;
        }

        [HttpGet("{id}")] // only DTO
        public virtual async Task<ActionResult> GetFile(Guid id)
        {
            return Ok(await _queue.Enqueue(new GetId<FileDto>(id)));
        }

        [HttpGet("download/{id}")] //only File
        public virtual async Task<ActionResult> DownloadFile(Guid id)
        {
            return Ok(await _queue.Enqueue(new GetFile(id)));
        }

        [HttpGet("filter/{property}")]
        public virtual async Task<ActionResult> GetFileFilter(string property, [FromQuery] string[] param)
        {
            return Ok(await _queue.Enqueue(new GetFilter<FileDto>(property, param)));
        }

        [HttpPost()]
        public virtual async Task<ActionResult> PostFile([FromForm] FileData data)
        {
            try
            {
                if (data != null)
                {
                    if (await _fileManager.TempSave(data.FileDto.FileNameWithExtencion, data.File))
                    {
                        return Ok(await _queue.Enqueue(new Add<FileDto>(data.FileDto)));
                    }
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
        public virtual async Task<ActionResult> DeleteFile(Guid id)
        {
            return Ok(await _queue.Enqueue(new Delete<FileDto>(id)));
        }

    }
}
