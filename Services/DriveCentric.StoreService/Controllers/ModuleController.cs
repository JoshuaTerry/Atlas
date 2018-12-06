using DriveCentric.BaseService.Controllers;
using DriveCentric.BaseService.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DriveCentric.ModuleService.Controllers
{
    [Produces("application/json")]
    public class ModuleController : BaseController<Module>
    {
        protected override string FieldsForAll => "Id,Name,Description,Category,Icon,Cost,IsOwned,DateCreated,DateModified";
        protected override string FieldsForSingle => "Id,Name,Description,Category,Icon,Cost,IsOwned,DateCreated,DateModified";
        protected override string FieldsForList => "Id,Name,Description,Category,Icon,Cost,IsOwned,DateCreated,DateModified";
        protected override string[] ReferenceFields => new string[] { };

        public ModuleController(
            IHttpContextAccessor httpContextAccessor,
            IContextInfoAccessor contextInfoAccessor,
            IBaseService<Module> service
            ) : base(httpContextAccessor, contextInfoAccessor, service)
        {
        }

        [MonitorAsyncAspect]
        [HttpGet]
        [Route("api/v1/module/{id}")]
        public async Task<IActionResult> Get(int id, string fields = null)
            => await GetSingle(x => x.Id == id, fields);

        [HttpGet]
        [Route("api/v1/module")]
        public async Task<IActionResult> GetAll()
            => await base.GetAll();

        [MonitorAsyncAspect]
        [HttpPost]
        [Route("api/v1/module")]
        public override async Task<IActionResult> Post(Module entity)
            => await base.Post(entity);

        [HttpPatch()]
        [Route("api/v1/module/{id}")]
        public override async Task<IActionResult> Patch(int id, JsonPatchDocument<Module> patch)
            => await base.Patch(id, patch);

        [MonitorAsyncAspect]
        [HttpDelete()]
        [Route("api/v1/module/{id}")]
        public override async Task<IActionResult> Delete(int id) => await base.Delete(id);
    }
}