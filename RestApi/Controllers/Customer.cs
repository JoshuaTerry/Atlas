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
//    [Produces("application/json")]
//    [Route("api/v1/customer")]
//    public class Customer : BaseController
//    {
//        private readonly ICustomerService customerService;

//        public Customer(
//            IHttpContextAccessor httpContextAccessor,
//            IContextInfoAccessor contextInfoAccessor,
//            ICustomerService customerService
//            ) : base(httpContextAccessor, contextInfoAccessor)
//        {
//            this.customerService = customerService;
//        }

//        // GET: api/v1/customer?limit={limit}&offset={offset}
//        [MonitorAsyncAspect]
//        [Authorize]
//        [HttpGet]
//        public async Task<IActionResult> Get([FromQuery] int? limit, [FromQuery] int? offset)
//        {
//            if (!ModelState.IsValid)
//            {
//                Log.Warning($"Invalid state getting {GetType().Name}.");
//                return BadRequest(ModelState);
//            }

//            try
//            {
//                var claims = User.Claims.ToList();

//                return Ok(await customerService.GetAsync(limit, offset));
//            }
//            catch (Exception exception)
//            {
//                return ExceptionHelper.ProcessError(exception);
//            }
//        }

//        // GET: api/v1/customer/id
//        [MonitorAsyncAspect]
//        [Authorize]
//        [HttpGet("{id}")]
//        public async Task<IActionResult> Get(int id)
//        {
//            if (!ModelState.IsValid)
//            {
//                Log.Warning($"Invalid state getting {GetType().Name}({id}).");
//                return BadRequest(ModelState);
//            }

//            try
//            {
//                var claims = User.Claims.ToList();

//                return Ok(await customerService.GetAsync(id));
//            }
//            catch (Exception exception)
//            {
//                return ExceptionHelper.ProcessError(exception);
//            }
//        }

//        // POST: api/v1/customer
//        //[HttpPost]
//        //public void Post([FromBody]string value)
//        //{
//        //}

//        // PUT: api/v1/customer/id
//        //[HttpPut("{id}")]
//        //public void Put(int id, [FromBody]string value)
//        //{
//        //}

//        // DELETE: api/v1/customer/id
//        [MonitorAsyncAspect]
//        [HttpDelete("{id}")]
//        [Authorize]
//        public async Task<IActionResult> Delete(int id)
//        {
//            if (!ModelState.IsValid)
//            {
//                Log.Warning($"Invalid state deleting {GetType().Name}({id}).");
//                return BadRequest(ModelState);
//            }

//            try
//            {
//                return Ok(await customerService.DeleteAsync(id));
//            }
//            catch (Exception exception)
//            {
//                return ExceptionHelper.ProcessError(exception);
//            }
//        }
//    }
}
