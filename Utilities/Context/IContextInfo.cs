using System.Security.Claims;

namespace DriveCentric.Utilities.Context
{
    public interface IContextInfo
    {
        ClaimsPrincipal User { get; }
        int? Identifier { get; }
    }
}
