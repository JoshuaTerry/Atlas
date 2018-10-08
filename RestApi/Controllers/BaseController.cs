using DriveCentric.RestApi.Context;
using DriveCentric.Utilities.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DriveCentric.RestApi.Controllers
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
