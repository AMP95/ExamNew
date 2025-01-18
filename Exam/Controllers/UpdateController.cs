using DTOs;
using Exam.BackgroundServices;
using MediatRepos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        private UpdateService _updateService;
        private ILogger<UpdateController> _logger;
        public UpdateController(UpdateService updateService, ILogger<UpdateController> logger)
        {
            _updateService = updateService;
            _logger = logger;
        }

        [HttpPut("vehicle")]
        public virtual async Task<ActionResult> PutVehicle([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _updateService.Add(new Update<VehicleDto>(jobj.ToObject<VehicleDto>())));
                }
                _logger.LogWarning($"VEHICLE: Recieved null object");
                return BadRequest("Передан пустой параметр");
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("Неверный тип данных");
            }
        }

        [HttpPut("driver")]
        public virtual async Task<ActionResult> PutDriver([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _updateService.Add(new Update<DriverDto>(jobj.ToObject<DriverDto>())));
                }
                _logger.LogWarning($"DRIVER: Recieved null object");
                return BadRequest("Передан пустой параметр");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("Неверный тип данных");
            }
            
        }

        [HttpPut("carrier")]
        public virtual async Task<ActionResult> PutCarrier([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _updateService.Add(new Update<CarrierDto>(jobj.ToObject<CarrierDto>())));
                }
                _logger.LogWarning($"CARRIER: Recieved null object");
                return BadRequest("Передан пустой параметр");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("Неверный тип данных");
            }
            
        }

        [HttpPut("client")]
        public virtual async Task<ActionResult> PutClient([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _updateService.Add(new Update<ClientDto>(jobj.ToObject<ClientDto>())));
                }
                _logger.LogWarning($"CLIENT: Recieved null object");
                return BadRequest("Передан пустой параметр");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("Неверный тип данных");
            }

            
        }

        [HttpPut("contract")]
        public virtual async Task<ActionResult> PutContract([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _updateService.Add(new Update<ContractDto>(jobj.ToObject<ContractDto>())));
                }
                _logger.LogWarning($"CONTRACT: Recieved null object");
                return BadRequest("Передан пустой параметр");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("Неверный тип данных");
            }
            
        }

        [HttpPut("document")]
        public virtual async Task<ActionResult> PutDocument([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _updateService.Add(new Update<DocumentDto>(jobj.ToObject<DocumentDto>())));
                }
                _logger.LogWarning($"DOCUMENT: Recieved null object");
                return BadRequest("Передан пустой параметр");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("Неверный тип данных");
            }
            
        }

        [HttpPut("payment")]
        public virtual async Task<ActionResult> PutPayment([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _updateService.Add(new Update<PaymentDto>(jobj.ToObject<PaymentDto>())));
                }
                _logger.LogWarning($"PAYMENT: Recieved null object");
                return BadRequest("Передан пустой параметр");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("Неверный тип данных");
            }
            
        }
    }
}
