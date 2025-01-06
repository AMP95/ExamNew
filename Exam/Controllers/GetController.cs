using Exam.BackgroundServices;
using MediatRepos;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Sub;

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

        [HttpGet("truck/id/{id}")]
        public virtual async Task<ActionResult> GetTruck(Guid id)
        {
            return Ok(await _getService.Add(new GetId<Truck>(id)));
        }

        [HttpGet("trailer/id/{id}")]
        public virtual async Task<ActionResult> GetTrailer(Guid id)
        {
            return Ok(await _getService.Add(new GetId<Trailer>(id)));
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
            return Ok(await _getService.Add(new GetId<Document>(id)));
        }

        [HttpGet("route/id/{id}")]
        public virtual async Task<ActionResult> GetRoute(Guid id)
        {
            return Ok(await _getService.Add(new GetId<RoutePoint>(id)));
        }

        #endregion ID

        #region MainId

        [HttpGet("truck/main/{id}")]
        public virtual async Task<ActionResult> GetTruckMain(Guid id)
        {
            return Ok(await _getService.Add(new GetMainId<Truck>(id)));
        }

        [HttpGet("trailer/main/{id}")]
        public virtual async Task<ActionResult> GetTrailerMain(Guid id)
        {
            return Ok(await _getService.Add(new GetMainId<Trailer>(id)));
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

        [HttpGet("truck/search/{name}")]
        public virtual async Task<ActionResult> GetTruckSearch(string name)
        {
            return Ok(await _getService.Add(new Search<Truck>(name)));
        }

        [HttpGet("trailer/search/{name}")]
        public virtual async Task<ActionResult> GetTrailerSearch(string name)
        {
            return Ok(await _getService.Add(new Search<Trailer>(name)));
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

        #endregion Search

        #region Range

        [HttpGet("truck/range/{start}/{end}")]
        public virtual async Task<ActionResult> GetTruckRange(int start, int end)
        {
            return Ok(await _getService.Add(new GetRange<Truck>(start, end)));
        }

        [HttpGet("trailer/range/{start}/{end}")]
        public virtual async Task<ActionResult> GetTrailerRange(int start, int end)
        {
            return Ok(await _getService.Add(new GetRange<Trailer>(start, end)));
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

        [HttpGet("contract/range/{start}/{end}")]
        public virtual async Task<ActionResult> GetContractRange(int start, int end)
        {
            return Ok(await _getService.Add(new GetRange<Document>(start, end)));
        }

        #endregion Range
    }
}
