using DTOs;
using DTOs.Dtos;
using Exam.Interfaces;
using MediatRepos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Utilities.Interfaces;

namespace Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddController : ControllerBase
    {
        private IAddService _addService;
        private ILogger<AddController> _logger;
        private IFileManager _fileManager;
        public AddController(IAddService addService, ILogger<AddController> logger, IFileManager manager)
        {
            _addService = addService;
            _logger = logger;
            _fileManager = manager;
        }

        [HttpPost("vehicle")]
        public virtual async Task<ActionResult> PostVehicle([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _addService.Add(new Add<VehicleDto>(jobj.ToObject<VehicleDto>())));
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

        [HttpPost("driver")]
        public virtual async Task<ActionResult> PostDriver([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _addService.Add(new Add<DriverDto>(jobj.ToObject<DriverDto>())));
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

        [HttpPost("carrier")]
        public virtual async Task<ActionResult> PostCarrier([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _addService.Add(new Add<CarrierDto>(jobj.ToObject<CarrierDto>())));
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

        [HttpPost("company")]
        public virtual async Task<ActionResult> PostClient([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _addService.Add(new Add<CompanyDto>(jobj.ToObject<CompanyDto>())));
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

        [HttpPost("contract")]
        public virtual async Task<ActionResult> PostContract([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _addService.Add(new Add<ContractDto>(jobj.ToObject<ContractDto>())));
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

        [HttpPost("document")]
        public virtual async Task<ActionResult> PostDocument([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _addService.Add(new Add<DocumentDto>(jobj.ToObject<DocumentDto>())));
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

        [HttpPost("payment")]
        public virtual async Task<ActionResult> PostPayment([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _addService.Add(new Add<PaymentDto>(jobj.ToObject<PaymentDto>())));
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

        [HttpPost("template")]
        public virtual async Task<ActionResult> PostTemplate([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _addService.Add(new Add<TemplateDto>(jobj.ToObject<TemplateDto>())));
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

        [HttpPost("file")]
        public virtual async Task<ActionResult> PostFile([FromForm]FileData data)
        {
            try
            {
                if (data != null)
                {
                    if (await _fileManager.TempSave(data.FileDto.FileNameWithExtencion, data.File)) 
                    {
                        return Ok(await _addService.Add(new Add<FileDto>(data.FileDto)));
                    }
                }
                _logger.LogWarning($"FILE: Recieved null object");
                return BadRequest("Передан пустой параметр");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("Неверный тип данных");
            }
        }

        [HttpPost("logist")]
        public virtual async Task<ActionResult> PostLogist([FromBody] JObject jobj)
        {
            try
            {
                if (jobj != null)
                {
                    return Ok(await _addService.Add(new Add<LogistDto>(jobj.ToObject<LogistDto>())));
                }
                _logger.LogWarning($"LOGIST: Recieved null object");
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
