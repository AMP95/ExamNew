using Exam.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private IFileService _fileService;
        private ILogger<FileController> _logger;
        public FileController(IFileService fileService, ILogger<FileController> logger)
        {
            _logger = logger;
            _fileService = fileService;
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult> GetFile(Guid id)
        {
            return Ok(await _fileService.Add(FileMethod.Get, id));
        }


        [HttpPost()]
        public virtual async Task<ActionResult> PostFiles(IFormFileCollection files)
        {
            if (files != null && files.Any()) 
            {
                return Ok(await _fileService.Add(FileMethod.Add, Guid.Empty, files));
            }
            _logger.LogError(message: "FILE: Empty object recieved");
            return BadRequest("Передан пустой параметр");
        }


        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> DeleteFile(Guid id)
        {
            return Ok(await _fileService.Add(FileMethod.Delete, id));
        }


        [HttpPut("{id}")]
        public virtual async Task<ActionResult> PutFile(Guid id, IFormFile file)
        {
            if (file != null)
            {
                return Ok(await _fileService.Add(FileMethod.Update, id, new FormFileCollection() { file }));
            }
            _logger.LogError(message: "FILE: Empty object recieved");
            return BadRequest("Передан пустой параметр");
        }
    }
}
