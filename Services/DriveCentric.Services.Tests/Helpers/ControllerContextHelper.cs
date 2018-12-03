using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DriveCentric.Services.Tests.Helpers
{
    internal static class ControllerContextHelper
    {
        internal static ControllerContext CreateControllerContext()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim("custom:DriveServerId", "21"),
                    new Claim("custom:UserGuid", "AED370F8-EAED-4997-BB86-786F0890E511")
                }));

            return new ControllerContext() { HttpContext = new DefaultHttpContext() { User = user } };
        }
    }
}