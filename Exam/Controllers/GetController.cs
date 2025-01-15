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

        [HttpGet("company/id/{id}")]
        public virtual async Task<ActionResult> GetCompany(Guid id)
        {
            return Ok(await _getService.Add(new GetId<Carrier>(id)));
        }

        [HttpGet("document/id/{id}")]
        public virtual async Task<ActionResult> GetDocument(Guid id)
        {
            return Ok(await _getService.Add(new GetId<Document>(id)));
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

        #region MainId

        [HttpGet("vehicle/main/{id}")]
        public virtual async Task<ActionResult> GetVehicleMain(Guid id)
        {
            return Ok(await _getService.Add(new GetMainId<Vehicle>(id)));
        }

        [HttpGet("driver/main/{id}")]
        public virtual async Task<ActionResult> GetDriverMain(Guid id)
        {
            return Ok(await _getService.Add(new GetMainId<Driver>(id)));
        }
        [HttpGet("document/main/{id}")]
        public virtual async Task<ActionResult> GetDocumentMain(Guid id)
        {
            return Ok(await _getService.Add(new GetMainId<Document>(id)));
        }

        #endregion MainId

        #region Search

        [HttpGet("vehicle/search/{name}")]
        public virtual async Task<ActionResult> GetVehicleSearch(string name)
        {
            return Ok(await _getService.Add(new Search<Vehicle>(name)));
        }

        [HttpGet("driver/search/{name}")]
        public virtual async Task<ActionResult> GetDriverSearch(string name)
        {
            return Ok(await _getService.Add(new Search<Driver>(name)));
        }

        [HttpGet("carrier/search/{name}")]
        public virtual async Task<ActionResult> GetCarrierSearch(string name)
        {
            return Ok(await _getService.Add(new Search<Carrier>(name)));
        }

        [HttpGet("contract/search/{name}")]
        public virtual async Task<ActionResult> GetContractSearch(string name)
        {
            return Ok(await _getService.Add(new Search<Document>(name)));
        }

        [HttpGet("company/search/{name}")]
        public virtual async Task<ActionResult> GetCompanySearch(string name)
        {
            return Ok(await _getService.Add(new Search<Company>(name)));
        }

        #endregion Search

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
        

        [HttpGet("company/range/{start}/{end}")]
        public virtual async Task<ActionResult> GetCompanyRange(int start, int end)
        {
            return Ok(await _getService.Add(new GetRange<Company>(start, end)));
        }

        #endregion Range

        #region Filter

        [HttpPost("contract/filter/{name}")]
        public virtual async Task<ActionResult> GetContractFilter([FromRoute] string name, [FromBody] JObject jobj)
        {
            if (jobj != null)
            {
                return Ok(await _getService.Add(new ContractFilter(name, jobj["param"].ToArray())));
            }
            _logger.LogWarning($"CONTRACT: Recieved null object");
            return BadRequest("Передан пустой параметр");
        }

        #endregion Filter
    }
}
