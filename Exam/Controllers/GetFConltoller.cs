using Exam.BackgroundServices;
using MediatRepos;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Sub;

namespace Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetFConltoller : ControllerBase
    {
        private GetService _getService;
        private ILogger<GetFConltoller> _logger;
        public GetFConltoller(GetService getService, ILogger<GetFConltoller> logger)
        {
            _getService = getService;
            _logger = logger;
        }

        [HttpGet("truck/{property}/{value}")]
        public virtual async Task<ActionResult> GetTruck(string property, object value)
        {
            return Ok(await _getService.Add(new GetFiltered<Truck>(property, value)));
        }
        [HttpGet("trailer/{property}/{value}")]
        public virtual async Task<ActionResult> GetTrailer(string property, object value)
        {
            return Ok(await _getService.Add(new GetFiltered<Trailer>(property, value)));
        }

        [HttpGet("driver/{property}/{value}")]
        public virtual async Task<ActionResult> GetDriver(string property, object value)
        {
            return Ok(await _getService.Add(new GetFiltered<Driver>(property, value)));
        }

        [HttpGet("carrier/{property}/{value}")]
        public virtual async Task<ActionResult> GetCarrier(string property, object value)
        {
            return Ok(await _getService.Add(new GetFiltered<Carrier>(property, value)));
        }

        [HttpGet("company/{property}/{value}")]
        public virtual async Task<ActionResult> GetCompany(string property, object value)
        {
            return Ok(await _getService.Add(new GetFiltered<Carrier>(property, value)));
        }

        [HttpGet("document/{property}/{value}")]
        public virtual async Task<ActionResult> GetDocument(string property, object value)
        {
            return Ok(await _getService.Add(new GetFiltered<Document>(property, value)));
        }

        [HttpGet("contract/{property}/{value}")]
        public virtual async Task<ActionResult> GetContract(string property, object value)
        {
            return Ok(await _getService.Add(new GetFiltered<Document>(property, value)));
        }

        [HttpGet("route/{property}/{value}")]
        public virtual async Task<ActionResult> GetRoute(string property, object value)
        {
            return Ok(await _getService.Add(new GetFiltered<RoutePoint>(property, value)));
        }
    }
}
