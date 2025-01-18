using DTOs;
using Exam.BackgroundServices;
using MediatRepos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatchController : ControllerBase
    {
        private UpdateService _updateService;
        private ILogger<PatchController> _logger;
        public PatchController(UpdateService updateService, ILogger<PatchController> logger)
        {
            _updateService = updateService;
            _logger = logger;
        }

        [HttpPatch("contract/{id}")]
        public virtual async Task<ActionResult> PatchContract(Guid id, [FromBody] JArray updates)
        {
            try
            {
                if (updates != null)
                {
                    return Ok(await _updateService.Add(new UpdateProperty<ContractDto>(id, updates.ToObject<KeyValuePair<string, object>[]>())));
                }
                _logger.LogWarning($"CONTRACT: Recieved null object");
                return BadRequest("Передан пустой параметр");

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex,ex.Message);
                return BadRequest("Передан неверный тип данных");
            }
            
        }
    }
}
