using DriveCentric.BaseService.Context;
using DriveCentric.Utilities.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DriveCentric.BaseService.Controllers
{
    public abstract class BaseController : Controller, IContextAccessible
    {
        public IContextInfoAccessor ContextInfoAccessor { get; }

        protected BaseController(
            IHttpContextAccessor httpContextAccessor,
            IContextInfoAccessor contextInfoAccessor
            )
        {
            contextInfoAccessor.ContextInfo = new ContextInfo(httpContextAccessor);
            ContextInfoAccessor = contextInfoAccessor;
        }
    }
}
