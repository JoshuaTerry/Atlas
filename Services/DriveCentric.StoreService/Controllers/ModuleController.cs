﻿using System.Threading.Tasks;
using DriveCentric.BaseService.Controllers;
using DriveCentric.Model;
using DriveCentric.ModuleService.Services;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace DriveCentric.ModuleService.Controllers
{
    [Produces("application/json")]
    public class ModuleController : BaseController<DriveCentric.Model.Module>
    {
        protected override string FieldsForAll => "Id,Name,Description,Category,Icon,Cost,IsOwned,DateCreated,DateModified";
        protected override string FieldsForSingle => "Id,Name,Description,Category,Icon,Cost,IsOwned,DateCreated,DateModified";
        protected override string FieldsForList => "Id,Name,Description,Category,Icon,Cost,IsOwned,DateCreated,DateModified";
        protected override string[] ReferenceFields => new string[] {  };

        public ModuleController(
            IHttpContextAccessor httpContextAccessor,
            IContextInfoAccessor contextInfoAccessor,
            IModuleService moduleService
            ) : base(httpContextAccessor, contextInfoAccessor, moduleService)
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
        public override async Task<IActionResult> Post([FromBody] Model.Module item)
            => await Post(item);

        [HttpPatch()]
        [Route("api/v1/module/{id}")]
        public override async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<DriveCentric.Model.Module> patch)
            => await Patch(id, patch);

        [MonitorAsyncAspect]
        [HttpDelete()]
        [Route("api/v1/module/{id}")]
        public override async Task<IActionResult> Delete(int id) => await Delete(id);
    }
}
