using System;
using System.Linq;
using System.Threading.Tasks;
using DriveCentric.BaseService;
using DriveCentric.BaseService.Controllers;
using DriveCentric.BaseService.Controllers.BindingModels;
using DriveCentric.Model;
using DriveCentric.TaskService.Services;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using DriveCentric.BaseService;
using DriveCentric.Model;

namespace DriveCentric.TaskService.Controllers
{
    [Produces("application/json")]
    // [Route("api/v1/task")]
    public class TaskController : BaseController<ITask>
    {
        protected override string FieldsForAll => "Id,Customer,CreatedByUser,User,ActionType,DateDue,Notes";
        protected override string FieldsForSingle => FieldsForAll;
        protected override string FieldsForList => FieldsForAll;


        public TaskController(
            IHttpContextAccessor httpContextAccessor,
            IContextInfoAccessor contextInfoAccessor,
            ITaskService taskService
            ) : base(httpContextAccessor, contextInfoAccessor, taskService)
        {
            
        }

        // GET: api/v1/task?limit={limit}&offset={offset}
        [MonitorAsyncAspect]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int id, [FromQuery] int? limit = SearchParameters.LimitMax, [FromQuery] int? offset = SearchParameters.OffsetDefault, string orderBy = null, string fields = null)
        {
            var userGuid = User.Claims.First(c => c.Type == "custom:UserGuid");

            if (!ModelState.IsValid)
            {
                Log.Warning($"Invalid state getting {GetType().Name}.");
                return BadRequest(ModelState);
            } 

            try
            {
                return Ok(await Service.GetAsync(limit, offset));
            }
            catch (Exception exception)
            {
                return ExceptionHelper.ProcessError(exception);
            }
        }

        [MonitorAsyncAspect]
        [HttpGet]
        [Route("api/v1/task/user/{id}")]
        public async Task<IActionResult> GetByUser([FromQuery] int id, int? limit = SearchParameters.LimitMax,  int? offset = SearchParameters.OffsetDefault, string orderBy = null, string fields = null)
        {
            
            if (!ModelState.IsValid)
            {
                Log.Warning($"Invalid state getting {GetType().Name}.");
                return BadRequest(ModelState);
            }

            try
            {
                //return Ok(await Service.GetAllByExpressionAsync(t => t.UserId == id, new PageableSearch(offset, limit, orderBy), fields));
                return await GetAll(t => t.UserId == id, limit, offset, orderBy, fields);
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

                return Ok(await Service.GetAsync(id));
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
                return Ok(await Service.InsertAsync(value));
            }
            catch (Exception exception)
            {
                return ExceptionHelper.ProcessError(exception);
            }
        }

        // PATCH: api/v1/task/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<ITask> patch)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning($"Invalid state patching {GetType().Name}({id}).");
                return BadRequest(ModelState);
            }

            try
            {
                var task = await Service.GetAsync(id);
                patch.ApplyTo(task, ModelState);

                return Ok(await Service.UpdateAsync(task));
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
                return Ok(await Service.DeleteAsync(id));
            }
            catch (Exception exception)
            {
                return ExceptionHelper.ProcessError(exception);
            }
        }
    }
}
