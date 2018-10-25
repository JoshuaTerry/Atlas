using System.Security.Claims;
using DriveCentric.Utilities.Context;
using Microsoft.AspNetCore.Http;

namespace DriveCentric.BaseService.Context
{
    public class ContextInfo : IContextInfo
    {
        public ClaimsPrincipal User { get; private set; }

        public int? Identifier { get; private set; }

        public ContextInfo(IHttpContextAccessor httpContextAccessor)
        {
            User = httpContextAccessor.HttpContext?.User;
            Identifier = httpContextAccessor.HttpContext?.Request?.GetHashCode();
        }
    }
}
