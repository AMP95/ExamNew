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

        [HttpGet("truck/{id}")]
        public virtual async Task<ActionResult> GetTruck(Guid id)
        {
            return Ok(await _getService.Add(new GetId<Truck>(id)));
        }

        [HttpGet("trailer/{id}")]
        public virtual async Task<ActionResult> GetTrailer(Guid id)
        {
            return Ok(await _getService.Add(new GetId<Trailer>(id)));
        }

        [HttpGet("driver/{id}")]
        public virtual async Task<ActionResult> GetDriver(Guid id)
        {
            return Ok(await _getService.Add(new GetId<Driver>(id)));
        }

        [HttpGet("carrier/{id}")]
        public virtual async Task<ActionResult> GetCarrier(Guid id)
        {
            return Ok(await _getService.Add(new GetId<Carrier>(id)));
        }

        [HttpGet("company/{id}")]
        public virtual async Task<ActionResult> GetCompany(Guid id)
        {
            return Ok(await _getService.Add(new GetId<Carrier>(id)));
        }

        [HttpGet("document/{id}")]
        public virtual async Task<ActionResult> GetDocument(Guid id)
        {
            return Ok(await _getService.Add(new GetId<Document>(id)));
        }

        [HttpGet("contract/{id}")]
        public virtual async Task<ActionResult> GetContract(Guid id)
        {
            return Ok(await _getService.Add(new GetId<Document>(id)));
        }

        [HttpGet("route/{id}")]
        public virtual async Task<ActionResult> GetRoute(Guid id)
        {
            return Ok(await _getService.Add(new GetId<RoutePoint>(id)));
        }
    }
}
