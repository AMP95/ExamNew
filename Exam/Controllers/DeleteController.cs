using Exam.BackgroundServices;
using MediatRepos;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteController : ControllerBase
    {
        private UpdateService _updateService;
        private ILogger<AddController> _logger;
        public DeleteController(UpdateService updateService, ILogger<AddController> logger)
        {
            _updateService = updateService;
            _logger = logger;
        }

        [HttpDelete("vehicle/{id}")]
        public virtual async Task<ActionResult> DeleteVehicle(Guid id)
        {
            return Ok(await _updateService.Add(new Delete<Vehicle>(id)));
        }


        [HttpDelete("document/{id}")]
        public virtual async Task<ActionResult> DeleteDocument(Guid id)
        {
            return Ok(await _updateService.Add(new Delete<Document>(id)));
        }

        [HttpDelete("payment/{id}")]
        public virtual async Task<ActionResult> DeletePayment(Guid id)
        {
            return Ok(await _updateService.Add(new Delete<Payment>(id)));
        }

        [HttpDelete("driver/{id}")]
        public virtual async Task<ActionResult> DeleteDriver(Guid id)
        {
            return Ok(await _updateService.Add(new Delete<Driver>(id)));
        }

        [HttpDelete("carrier/{id}")]
        public virtual async Task<ActionResult> DeleteCarrier(Guid id)
        {
            return Ok(await _updateService.Add(new Delete<Carrier>(id)));
        }

        [HttpDelete("client/{id}")]
        public virtual async Task<ActionResult> DeleteClient(Guid id)
        {
            return Ok(await _updateService.Add(new Delete<Client>(id)));
        }
    }
}
