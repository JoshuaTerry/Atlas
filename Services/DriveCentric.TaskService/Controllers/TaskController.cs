using DriveCentric.BaseService.Controllers;
using DriveCentric.BaseService.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PostSharp.Patterns.Caching;
using System.Threading.Tasks;

namespace DriveCentric.TaskService.Controllers
{
    [Produces("application/json")]
    public class TaskController : BaseController<Core.Models.UserTask>
    {
        protected override string FieldsForAll => "Id,Customer,CreatedByUser,User,ActionType,DateDue,Notes";
        protected override string FieldsForSingle => "Id,Customer.FirstName";
        protected override string FieldsForList => "Id,Customer.FirstName,Notes";
        protected override string[] ReferenceFields => new string[] { "Customer", "User" };

        public TaskController(
            IHttpContextAccessor httpContextAccessor,
            IContextInfoAccessor contextInfoAccessor,
            IBaseService<DriveCentric.Core.Models.UserTask> service
            ) : base(httpContextAccessor, contextInfoAccessor, service) { }

        [Authorize]
        [HttpGet]
        [Route("api/v1/task/healthcheck")]
        public async Task<IActionResult> Get()
            => Ok(await Service.GetHealthCheck());

        [MonitorAsyncAspect(AspectPriority = 1)]
        [Cache(AspectPriority = 2)]
        [Authorize]
        [HttpGet]
        [Route("api/v1/task/{id}")]
        public async Task<IActionResult> Get(int id, string fields = null)
            => await GetSingle(x => x.Id == id, fields);

        [MonitorAsyncAspect(AspectPriority = 1)]
        [Cache(AspectPriority = 2)]
        [HttpGet]
        [Route("api/v1/task/user/{id}")]
        public async Task<IActionResult> GetByUser(
            int id,
            int? limit = SearchParameters.LimitMax,
            int? offset = SearchParameters.OffsetDefault,
            string orderBy = null,
            string fields = null
            )
            => await base.GetAll(x => x.UserId == id, limit, offset, orderBy, fields);

        [MonitorAsyncAspect]
        [HttpPost]
        [Route("api/v1/task")]
        public override async Task<IActionResult> Post([FromBody] Core.Models.UserTask entity)
            => await base.Post(entity);

        [HttpPatch()]
        [Route("api/v1/task/{id}")]
        public override async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<Core.Models.UserTask> patch)
            => await base.Patch(id, patch);

        [MonitorAsyncAspect]
        [HttpDelete()]
        [Route("api/v1/task/{id}")]
        public override async Task<IActionResult> Delete(int id) => await base.Delete(id);
    }
}