using DriveCentric.BaseService.Controllers;
using DriveCentric.BaseService.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PostSharp.Patterns.Caching;

namespace DriveCentric.CustomerServiceService.Controllers
{
    [Produces("application/json")]
    public class CustomerServiceController : BaseController<CustomerService>
    {
        protected override string FieldsForAll => "";
        protected override string FieldsForSingle => "";
        protected override string FieldsForList => "";
        protected override string[] ReferenceFields => new string[] { };

        public CustomerServiceController(
            IHttpContextAccessor httpContextAccessor,
            IContextInfoAccessor contextInfoAccessor,
            IBaseService<CustomerService> service
            ) : base(httpContextAccessor, contextInfoAccessor, service)
        {

        }

        [MonitorAsyncAspect(AspectPriority = 1)]
        [HttpGet]
        [Route("api/v1/CustomerService/{id}")]
        public async Task<IActionResult> Get(int id, string fields = null)
            => await GetSingle(x => x.Id == id, fields);

        [HttpGet]
        [Route("api/v1/CustomerService")]
        public async Task<IActionResult> GetAll()
            => await base.GetAll();

        [MonitorAsyncAspect(AspectPriority = 1)]
        [HttpPost]
        public override async Task<IActionResult> Post([FromBody] CustomerService item)
            => await base.Post(item);

        [HttpPatch()]
        [Route("api/v1/CustomerService/{id}")]
        public override async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<CustomerService> patch)
            => await base.Patch(id, patch);

        [MonitorAsyncAspect(AspectPriority = 1)]
        [HttpDelete()]
        [Route("api/v1/CustomerService/{id}")]
        public override async Task<IActionResult> Delete(int id) => await base.Delete(id);
    }
}
