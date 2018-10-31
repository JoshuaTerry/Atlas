using System;
using System.Linq;
using System.Threading.Tasks;
using DriveCentric.BaseService;
using DriveCentric.BaseService.Controllers;
using DriveCentric.BaseService.Controllers.BindingModels;
using DriveCentric.Model;
using DriveCentric.Task.Services;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DriveCentric.Task.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/task")]
    public class TaskController : BaseController
    {
        private readonly ITaskService taskService;

        public TaskController(
            IHttpContextAccessor httpContextAccessor,
            IContextInfoAccessor contextInfoAccessor,
            ITaskService taskService
            ) : base(httpContextAccessor, contextInfoAccessor)
        {
            this.taskService = taskService;
        }

        // GET: api/v1/task?limit={limit}&offset={offset}
        [MonitorAsyncAspect]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning($"Invalid state getting {GetType().Name}.");
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(await taskService.GetAsync(limit, offset));
            }
            catch (Exception exception)
            {
                return ExceptionHelper.ProcessError(exception);
            }
        }

        // GET: api/v1/task/5
        [MonitorAsyncAspect]
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning($"Invalid state getting {GetType().Name}({id}).");
                return BadRequest(ModelState);
            }

            try
            {
                var claims = User.Claims.ToList();

                return Ok(await taskService.GetAsync(id));
            }
            catch (Exception exception)
            {
                return ExceptionHelper.ProcessError(exception);
            }
        }

        // POST: api/task
        [MonitorAsyncAspect]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskBindingModel value)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning($"Invalid state adding new {GetType().Name}.");
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(await taskService.InsertAsync(value));
            }
            catch (Exception exception)
            {
                return ExceptionHelper.ProcessError(exception);
            }
        }

        // PUT: api/task/5
        [MonitorAsyncAspect]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TaskBindingModel value)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning($"Invalid state updating {GetType().Name}({id}).");
                return BadRequest(ModelState);
            }

            try
            {
                var existingItem = await taskService.GetAsync(id);

                // update the changes

                return Ok(await taskService.UpdateAsync(existingItem));
            }
            catch (Exception exception)
            {
                return ExceptionHelper.ProcessError(exception);
            }
        }

        // DELETE: api/v1/task/5
        [MonitorAsyncAspect]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning($"Invalid state deleting {GetType().Name}({id}).");
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(await taskService.DeleteAsync(id));
            }
            catch (Exception exception)
            {
                return ExceptionHelper.ProcessError(exception);
            }
        }
    }
}
