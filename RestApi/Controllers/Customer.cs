using System;
using System.Linq;
using System.Threading.Tasks;
using DriveCentric.ServiceLayer.Customer;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DriveCentric.RestApi.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/Customers")]
    public class Customer : BaseController
    {
        private readonly ICustomerService customerService;

        public Customer(
            IHttpContextAccessor httpContextAccessor,
            IContextInfoAccessor contextInfoAccessor,
            ICustomerService customerService
            ) : base(httpContextAccessor, contextInfoAccessor)
        {
            this.customerService = customerService;
        }

        // GET: api/v1/customers?limit={limit}&offset={offset}
        [MonitorAsyncAspect]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning($"Invalid state getting {this.GetType().Name}.");
                return BadRequest(ModelState);
            }

            try
            {
                var claims = User.Claims.ToList();

                return Ok(await customerService.GetCustomersAsync(limit, offset));
            }
            catch (Exception exception)
            {
                return ExceptionHelper.ProcessError(exception);
            }
        }

        // GET: api/v1/customers/id
        [MonitorAsyncAspect]
        //[Authorize]
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

                return Ok(await customerService.GetCustomerAsync(id));
            }
            catch (Exception exception)
            {
                return ExceptionHelper.ProcessError(exception);
            }
        }

        // POST: api/v1/customers
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        // PUT: api/v1/customers/id
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE: api/v1/customers/id
        [MonitorAsyncAspect]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning($"Invalid state deleting {this.GetType().Name}({id}).");
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(await customerService.DeleteCustomerAsync(id));
            }
            catch (Exception exception)
            {
                return ExceptionHelper.ProcessError(exception);
            }
        }
    }
}
