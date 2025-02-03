using DTOs;
using DTOs.Dtos;
using Exam.Interfaces;
using MediatRepos;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteController : ControllerBase
    {
        private IUpdateService _updateService;
        public DeleteController(IUpdateService updateService)
        {
            _updateService = updateService;
        }

        [HttpDelete("vehicle/{id}")]
        public virtual async Task<ActionResult> DeleteVehicle(Guid id)
        {
            return Ok(await _updateService.Add(new Delete<VehicleDto>(id)));
        }

        [HttpDelete("document/{id}")]
        public virtual async Task<ActionResult> DeleteDocument(Guid id)
        {
            return Ok(await _updateService.Add(new Delete<DocumentDto>(id)));
        }

        [HttpDelete("payment/{id}")]
        public virtual async Task<ActionResult> DeletePayment(Guid id)
        {
            return Ok(await _updateService.Add(new Delete<PaymentDto>(id)));
        }

        [HttpDelete("driver/{id}")]
        public virtual async Task<ActionResult> DeleteDriver(Guid id)
        {
            return Ok(await _updateService.Add(new Delete<DriverDto>(id)));
        }

        [HttpDelete("carrier/{id}")]
        public virtual async Task<ActionResult> DeleteCarrier(Guid id)
        {
            return Ok(await _updateService.Add(new Delete<CarrierDto>(id)));
        }

        [HttpDelete("client/{id}")]
        public virtual async Task<ActionResult> DeleteClient(Guid id)
        {
            return Ok(await _updateService.Add(new Delete<CompanyDto>(id)));
        }

        [HttpDelete("file/{id}")]
        public virtual async Task<ActionResult> DeleteFile(Guid id)
        {
            return Ok(await _updateService.Add(new Delete<FileDto>(id)));
        }

        [HttpDelete("template/{id}")]
        public virtual async Task<ActionResult> DeleteTemplate(Guid id)
        {
            return Ok(await _updateService.Add(new Delete<TemplateDto>(id)));
        }
    }
}
