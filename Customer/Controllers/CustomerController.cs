using System.Threading.Tasks;
using DriveCentric.BaseService.Controllers;
using DriveCentric.BaseService.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace DriveCentric.CustomerService.Controllers
{
    [Produces("application/json")]
    public class CustomerController : BaseController<Customer>
    {
        protected override string FieldsForAll => "";
        protected override string FieldsForSingle => "";
        protected override string FieldsForList => "";
        protected override string[] ReferenceFields => new string[] { };

        public CustomerController(
            IHttpContextAccessor httpContextAccessor,
            IContextInfoAccessor contextInfoAccessor,
            IBaseService<Customer> service
            ) : base(httpContextAccessor, contextInfoAccessor, service)
        {
        }

        [MonitorAsyncAspect(AspectPriority = 1)]
        [HttpGet]
        [Route("api/v1/Customer/{id}")]
        public async Task<IActionResult> Get(int id, string fields = null)
            => await GetSingle(x => x.Id == id, fields);

        [HttpGet]
        [Route("api/v1/Customer")]
        public async Task<IActionResult> GetAll()
            => await base.GetAll();

        [MonitorAsyncAspect(AspectPriority = 1)]
        [HttpPost]
        public override async Task<IActionResult> Post([FromBody] Customer item)
            => await base.Post(item);

        [HttpPatch()]
        [Route("api/v1/Customer/{id}")]
        public override async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<Customer> patch)
            => await base.Patch(id, patch);

        [MonitorAsyncAspect(AspectPriority = 1)]
        [HttpDelete()]
        [Route("api/v1/Customer/{id}")]
        public override async Task<IActionResult> Delete(int id) => await base.Delete(id);
    }
}