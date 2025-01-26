﻿using DTOs;
using DTOs.Dtos;
using Exam.Interfaces;
using MediatRepos;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetController : ControllerBase
    {
        private IGetService _getService;
        public GetController(IGetService getService)
        {
            _getService = getService;
        }

        #region ID

        [HttpGet("vehicle/id/{id}")]
        public virtual async Task<ActionResult> GetVehicle(Guid id)
        {
            return Ok(await _getService.Add(new GetId<VehicleDto>(id)));
        }

        [HttpGet("driver/id/{id}")]
        public virtual async Task<ActionResult> GetDriver(Guid id)
        {
            return Ok(await _getService.Add(new GetId<DriverDto>(id)));
        }

        [HttpGet("carrier/id/{id}")]
        public virtual async Task<ActionResult> GetCarrier(Guid id)
        {
            return Ok(await _getService.Add(new GetId<CarrierDto>(id)));
        }

        [HttpGet("client/id/{id}")]
        public virtual async Task<ActionResult> GetClient(Guid id)
        {
            return Ok(await _getService.Add(new GetId<ClientDto>(id)));
        }

        [HttpGet("document/id/{id}")]
        public virtual async Task<ActionResult> GetDocument(Guid id)
        {
            return Ok(await _getService.Add(new GetId<DocumentDto>(id)));
        }

        [HttpGet("payment/id/{id}")]
        public virtual async Task<ActionResult> GetPayment(Guid id)
        {
            return Ok(await _getService.Add(new GetId<PaymentDto>(id)));
        }

        [HttpGet("contract/id/{id}")]
        public virtual async Task<ActionResult> GetContract(Guid id)
        {
            return Ok(await _getService.Add(new GetId<ContractDto>(id)));
        }


        [HttpGet("template/id/{id}")]
        public virtual async Task<ActionResult> GetTemplate(Guid id)
        {
            return Ok(await _getService.Add(new GetId<ContractTemplateDto>(id)));
        }

        [HttpGet("file/id/{id}")] // only DTO
        public virtual async Task<ActionResult> GetFile(Guid id)
        {
            return Ok(await _getService.Add(new GetId<FileDto>(id)));
        }

        [HttpGet("file/download/{id}")] //only File
        public virtual async Task<ActionResult> DownloadFile(Guid id)
        {
            return Ok(await _getService.Add(new GetFile(id)));
        }


        #endregion ID

        #region Filter

        [HttpGet("contract/filter/{property}")]
        public virtual async Task<ActionResult> GetContractFilter(string property, [FromQuery] string[] param)
        {
            return Ok(await _getService.Add(new GetFilter<ContractDto>(property, param)));
        }

        [HttpGet("contract/payment")]
        public virtual async Task<ActionResult> GetContractPayment()
        {
            return Ok(await _getService.Add(new GetRequiredToPay()));
        }

        [HttpGet("vehicle/filter/{property}")]
        public virtual async Task<ActionResult> GetVehicleFilter(string property, [FromQuery] string[] param)
        {
            return Ok(await _getService.Add(new GetFilter<VehicleDto>(property, param)));
        }

        [HttpGet("driver/filter/{property}")]
        public virtual async Task<ActionResult> GetDriverFilter(string property, [FromQuery] string[] param)
        {
            return Ok(await _getService.Add(new GetFilter<DriverDto>(property, param)));
        }

        [HttpGet("document/filter/{property}")]
        public virtual async Task<ActionResult> GetDocumentFilter(string property, [FromQuery] string[] param)
        {
            return Ok(await _getService.Add(new GetFilter<DocumentDto>(property, param)));
        }

        [HttpGet("payment/filter/{property}")]
        public virtual async Task<ActionResult> GetPaymentFilter(string property, [FromQuery] string[] param)
        {
            return Ok(await _getService.Add(new GetFilter<PaymentDto>(property, param)));
        }

        [HttpGet("carrier/filter/{property}")]
        public virtual async Task<ActionResult> GetCarrierFilter(string property, [FromQuery] string[] param)
        {
            return Ok(await _getService.Add(new GetFilter<CarrierDto>(property, param)));
        }

        [HttpGet("client/filter/{property}")]
        public virtual async Task<ActionResult> GetClientFilter(string property, [FromQuery] string[] param)
        {
            return Ok(await _getService.Add(new GetFilter<ClientDto>(property, param)));
        }

        [HttpGet("file/filter/{property}")]
        public virtual async Task<ActionResult> GetFileFilter(string property, [FromQuery] string[] param)
        {
            return Ok(await _getService.Add(new GetFilter<FileDto>(property, param)));
        }

        [HttpGet("template/filter/{property}")]
        public virtual async Task<ActionResult> GetTemplateFilter(string property, [FromQuery] string[] param)
        {
            return Ok(await _getService.Add(new GetFilter<ContractTemplateDto>(property, param)));
        }

        [HttpGet("bookmark/filter/{property}")]
        public virtual async Task<ActionResult> GetBookmarkFilter(string property, [FromQuery] string[] param)
        {
            return Ok(await _getService.Add(new GetFilter<BookMarkDto>(property, param)));
        }

        #endregion Filter

        #region Range

        [HttpGet("vehicle/range/{start}/{end}")]
        public virtual async Task<ActionResult> GetVehicleRange(int start, int end)
        {
            return Ok(await _getService.Add(new GetRange<VehicleDto>(start, end)));
        }

        [HttpGet("driver/range/{start}/{end}")]
        public virtual async Task<ActionResult> GetDriverRange(int start, int end)
        {
            return Ok(await _getService.Add(new GetRange<DriverDto>(start, end)));
        }

        [HttpGet("carrier/range/{start}/{end}")]
        public virtual async Task<ActionResult> GetCarrierRange(int start, int end)
        {
            return Ok(await _getService.Add(new GetRange<CarrierDto>(start, end)));
        }
        

        [HttpGet("client/range/{start}/{end}")]
        public virtual async Task<ActionResult> GetClientRange(int start, int end)
        {
            return Ok(await _getService.Add(new GetRange<ClientDto>(start, end)));
        }

        [HttpGet("template/range/{start}/{end}")]
        public virtual async Task<ActionResult> GetTemplateRange(int start, int end)
        {
            return Ok(await _getService.Add(new GetRange<ContractTemplateDto>(start, end)));
        }

        #endregion Range
    }
}
