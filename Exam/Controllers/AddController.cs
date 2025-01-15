using DTOs;
using Exam.BackgroundServices;
using MediatRepos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddController : ControllerBase
    {
        private UpdateService _updateService;
        private ILogger<AddController> _logger;
        public AddController(UpdateService updateService, ILogger<AddController> logger)
        {
            _updateService = updateService;
            _logger = logger;
        }

        [HttpPost("vehicle")]
        public virtual async Task<ActionResult> PostVehicle([FromBody] JObject jobj)
        {
            if (jobj != null)
            {
                return Ok(await _updateService.Add(new Add<VehicleDto>(jobj.ToObject<VehicleDto>())));
            }
            _logger.LogWarning($"VEHICLE: Recieved null object");
            return BadRequest("Передан пустой параметр");
        }


        [HttpPost("driver")]
        public virtual async Task<ActionResult> PostDriver([FromBody] JObject jobj)
        {
            if (jobj != null)
            {
                return Ok(await _updateService.Add(new Add<DriverDto>(jobj.ToObject<DriverDto>())));
            }
            _logger.LogWarning($"DRIVER: Recieved null object");
            return BadRequest("Передан пустой параметр");
        }

        [HttpPost("carrier")]
        public virtual async Task<ActionResult> PostCarrier([FromBody] JObject jobj)
        {
            if (jobj != null)
            {
                return Ok(await _updateService.Add(new Add<CarrierDto>(jobj.ToObject<CarrierDto>())));
            }
            _logger.LogWarning($"CARRIER: Recieved null object");
            return BadRequest("Передан пустой параметр");
        }

        [HttpPost("client")]
        public virtual async Task<ActionResult> PostClient([FromBody] JObject jobj)
        {
            if (jobj != null)
            {
                return Ok(await _updateService.Add(new Add<ClientDto>(jobj.ToObject<ClientDto>())));
            }
            _logger.LogWarning($"CLIENT: Recieved null object");
            return BadRequest("Передан пустой параметр");
        }

        [HttpPost("contract")]
        public virtual async Task<ActionResult> PostContract([FromBody] JObject jobj)
        {
            if (jobj != null)
            {
                return Ok(await _updateService.Add(new Add<ContractDto>(jobj.ToObject<ContractDto>())));
            }
            _logger.LogWarning($"CONTRACT: Recieved null object");
            return BadRequest("Передан пустой параметр");
        }

        [HttpPost("document")]
        public virtual async Task<ActionResult> PostDocument([FromBody] JObject jobj)
        {
            if (jobj != null)
            {
                return Ok(await _updateService.Add(new Add<DocumentDto>(jobj.ToObject<DocumentDto>())));
            }
            _logger.LogWarning($"DOCUMENT: Recieved null object");
            return BadRequest("Передан пустой параметр");
        }

        [HttpPost("payment")]
        public virtual async Task<ActionResult> PostPayment([FromBody] JObject jobj)
        {
            if (jobj != null)
            {
                return Ok(await _updateService.Add(new Add<PaymentDto>(jobj.ToObject<PaymentDto>())));
            }
            _logger.LogWarning($"PAYMENT: Recieved null object");
            return BadRequest("Передан пустой параметр");
        }
    }
}
