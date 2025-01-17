using DTOs;
using Exam.BackgroundServices;
using MediatRepos;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Sub;
using Newtonsoft.Json.Linq;

namespace Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetController : ControllerBase
    {
        private GetService _getService;
        private ILogger<GetController> _logger;
        public GetController(GetService getService, ILogger<GetController> logger)
        {
            _getService = getService;
            _logger = logger;
        }

        #region ID

        [HttpGet("vehicle/id/{id}")]
        public virtual async Task<ActionResult> GetVehicle(Guid id)
        {
            return Ok(await _getService.Add(new GetId<Vehicle>(id)));
        }

        [HttpGet("driver/id/{id}")]
        public virtual async Task<ActionResult> GetDriver(Guid id)
        {
            return Ok(await _getService.Add(new GetId<Driver>(id)));
        }

        [HttpGet("carrier/id/{id}")]
        public virtual async Task<ActionResult> GetCarrier(Guid id)
        {
            return Ok(await _getService.Add(new GetId<Carrier>(id)));
        }

        [HttpGet("client/id/{id}")]
        public virtual async Task<ActionResult> GetClient(Guid id)
        {
            return Ok(await _getService.Add(new GetId<Client>(id)));
        }

        [HttpGet("document/id/{id}")]
        public virtual async Task<ActionResult> GetDocument(Guid id)
        {
            return Ok(await _getService.Add(new GetId<Document>(id)));
        }

        [HttpGet("payment/id/{id}")]
        public virtual async Task<ActionResult> GetPayment(Guid id)
        {
            return Ok(await _getService.Add(new GetId<Payment>(id)));
        }

        [HttpGet("contract/id/{id}")]
        public virtual async Task<ActionResult> GetContract(Guid id)
        {
            return Ok(await _getService.Add(new GetId<Contract>(id)));
        }

        [HttpGet("route/id/{id}")]
        public virtual async Task<ActionResult> GetRoute(Guid id)
        {
            return Ok(await _getService.Add(new GetId<RoutePoint>(id)));
        }

        #endregion ID

        #region Filter

        [HttpGet("contract/filter/{property}")]
        public virtual async Task<ActionResult> GetContractFilter(string property, [FromQuery] string[] param)
        {
            return Ok(await _getService.Add(new GetFilter<Contract>(property, param)));
        }

        [HttpGet("vehicle/filter/")]
        public virtual async Task<ActionResult> GetVehicleFilter([FromQuery] string property, [FromQuery] string[] param)
        {
            return Ok(await _getService.Add(new GetFilter<Vehicle>(property, param)));
        }

        [HttpGet("driver/filter/")]
        public virtual async Task<ActionResult> GetDriverFilter([FromQuery] string property, [FromQuery] string[] param)
        {
            return Ok(await _getService.Add(new GetFilter<Driver>(property, param)));
        }

        [HttpGet("document/filter/")]
        public virtual async Task<ActionResult> GetDocumentFilter([FromQuery] string property, [FromQuery] string[] param)
        {
            return Ok(await _getService.Add(new GetFilter<Document>(property, param)));
        }

        [HttpGet("payment/filter/")]
        public virtual async Task<ActionResult> GetPaymentFilter([FromQuery] string property, [FromQuery] string[] param)
        {
            return Ok(await _getService.Add(new GetFilter<Payment>(property, param)));
        }

        [HttpGet("carrier/filter/")]
        public virtual async Task<ActionResult> GetCarrierFilter([FromQuery] string property, [FromQuery] string[] param)
        {
            return Ok(await _getService.Add(new GetFilter<Carrier>(property, param)));
        }

        [HttpGet("client/filter/")]
        public virtual async Task<ActionResult> GetClientFilter([FromQuery] string property, [FromQuery] string[] param)
        {
            return Ok(await _getService.Add(new GetFilter<Client>(property, param)));
        }

        #endregion Filter

        #region Range

        [HttpGet("vehicle/range/{start}/{end}")]
        public virtual async Task<ActionResult> GetVehicleRange(int start, int end)
        {
            return Ok(await _getService.Add(new GetRange<Vehicle>(start, end)));
        }

        [HttpGet("driver/range/{start}/{end}")]
        public virtual async Task<ActionResult> GetDriverRange(int start, int end)
        {
            return Ok(await _getService.Add(new GetRange<Driver>(start, end)));
        }

        [HttpGet("carrier/range/{start}/{end}")]
        public virtual async Task<ActionResult> GetCarrierRange(int start, int end)
        {
            return Ok(await _getService.Add(new GetRange<Carrier>(start, end)));
        }
        

        [HttpGet("client/range/{start}/{end}")]
        public virtual async Task<ActionResult> GetClientRange(int start, int end)
        {
            return Ok(await _getService.Add(new GetRange<Client>(start, end)));
        }

        #endregion Range
    }
}
