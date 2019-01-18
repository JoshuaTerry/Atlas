using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace DriveCentric.Utilities.Context
{
    public class ContextInfo : IContextInfo
    {
        public ContextInfo(IHttpContextAccessor httpContextAccessor)
        {
            User = httpContextAccessor.HttpContext?.User;
            Identifier = httpContextAccessor.HttpContext?.Request?.GetHashCode();
        }

        public ClaimsPrincipal User { get; }

        public int? Identifier { get; }
    }
}