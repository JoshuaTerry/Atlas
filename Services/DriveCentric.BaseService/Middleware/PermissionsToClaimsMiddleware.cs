using DriveCentric.BaseService.Services;
using DriveCentric.BusinessLogic.Implementation;
using DriveCentric.Core.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Utilities.Context;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DriveCentric.BaseService.Middleware
{
    public class PermissionsToClaimsMiddleware
    {
        private readonly RequestDelegate next;

        public PermissionsToClaimsMiddleware(
            RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IContextInfoAccessor contextInfoAccessor,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                try
                {
                    var claims = context.User.Claims;
                    var user = context.User.Identity as ClaimsIdentity;

                    var userGuidClaim = claims.FirstOrDefault(claim => claim.Type == "custom:UserGuid")?.Value;
                    Guid userGuid = new Guid(userGuidClaim);
                    var driveServerIdClaim = claims.FirstOrDefault(claim => claim.Type == "custom:DriveServerId")?.Value;

                    contextInfoAccessor.ContextInfo = new ContextInfo(httpContextAccessor);

                    await AddGalaxyClaims(contextInfoAccessor, unitOfWork, user, userGuid);
                    await AddStarClaims(contextInfoAccessor, unitOfWork, user, userGuid);
                }
                catch (Exception exception)
                {
                    Log.Error(exception, "Error adding claims to the user.");
                }
            }

            await next(context);
        }

        private static async System.Threading.Tasks.Task AddStarClaims(
            IContextInfoAccessor contextInfoAccessor,
            IUnitOfWork unitOfWork,
            ClaimsIdentity user,
            Guid userGuid)
        {
            var starService = new BaseService<StarClaimPermission>(contextInfoAccessor, unitOfWork, new NoLogic<StarClaimPermission>(null));
            var starPermissions =
                await starService.GetAllByExpressionAsync(
                    x => x.ExternalId == userGuid, PageableSearch.Max);

            foreach (var permission in starPermissions.Data)
            {
                user.AddClaim(new Claim(permission.Key, permission.Value));
            }
        }

        private static async System.Threading.Tasks.Task AddGalaxyClaims(
            IContextInfoAccessor contextInfoAccessor,
            IUnitOfWork unitOfWork,
            ClaimsIdentity user,
            Guid userGuid)
        {
            var service = new BaseService<GalaxyClaimPermission>(contextInfoAccessor, unitOfWork, new NoLogic<GalaxyClaimPermission>(null));
            var permissions =
                await service.GetAllByExpressionAsync(
                    x => x.ExternalId == userGuid, PageableSearch.Max);

            foreach (var permission in permissions.Data)
            {
                user.AddClaim(new Claim(permission.Key, permission.Value));
            }
        }
    }
}