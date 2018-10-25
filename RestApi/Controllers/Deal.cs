using System;
using System.Linq;
using System.Threading.Tasks;
using DriveCentric.BaseService.Controllers;
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
    [Route("api/v1/deal")]
    public class Deal : BaseController
    {
        private readonly IDealService dealService;

        public Deal(
            IHttpContextAccessor httpContextAccessor,
            IContextInfoAccessor contextInfoAccessor,
            IDealService dealService
            ) : base(httpContextAccessor, contextInfoAccessor)
        {
            this.dealService = dealService;
        }

        //// GET: api/v1/Deal
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/v1/Deal/id
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
                var claims = User.Claims.ToList();

                return Ok(await dealService.GetAsync(id));
            }
            catch (Exception exception)
            {
                return ExceptionHelper.ProcessError(exception);
            }
        }

        //// POST: api/v1/Deal
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT: api/v1/Deal/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/v1/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
