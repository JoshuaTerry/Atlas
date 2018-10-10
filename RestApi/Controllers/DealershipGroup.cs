using System;
using System.Threading.Tasks;
using DriveCentric.ServiceLayer.Interfaces;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DriveCentric.RestApi.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/dealershipgroup")]
    public class DealershipGroup : BaseController
    {
        private readonly IDealershipGroupService dealershipGroupService;

        public DealershipGroup(
            IHttpContextAccessor httpContextAccessor,
            IContextInfoAccessor contextInfoAccessor,
            IDealershipGroupService dealershipGroupService
            ) : base(httpContextAccessor, contextInfoAccessor)
        {
            this.dealershipGroupService = dealershipGroupService;
        }

        // GET: api/v1/DealershipGroup
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/v1/DealershipGroup/5
        [MonitorAsyncAspect]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning($"Invalid state getting {this.GetType().Name}({id}).");
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(await dealershipGroupService.GetAsync(id));
            }
            catch (Exception exception)
            {
                return ExceptionHelper.ProcessError(exception);
            }
        }

        // POST: api/v1/DealershipGroup
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        // PUT: api/v1/DealershipGroup/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE: api/v1/dealershipgroup/5
        [MonitorAsyncAspect]
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning($"Invalid state deleting {this.GetType().Name}({id}).");
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(await dealershipGroupService.DeleteAsync(id));
            }
            catch (Exception exception)
            {
                return ExceptionHelper.ProcessError(exception);
            }
        }
    }
}
