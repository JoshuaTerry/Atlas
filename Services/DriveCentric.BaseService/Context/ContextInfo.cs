using DriveCentric.Utilities.Context;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DriveCentric.BaseService.Context
{
    public class ContextInfo : IContextInfo
    {
        public ClaimsPrincipal User { get; }

        public int? Identifier { get; }

        public ContextInfo(IHttpContextAccessor httpContextAccessor)
        {
            User = httpContextAccessor.HttpContext?.User;
            Identifier = httpContextAccessor.HttpContext?.Request?.GetHashCode();
        }
    }
}