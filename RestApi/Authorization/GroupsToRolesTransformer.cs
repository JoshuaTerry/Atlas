using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Okta.Sdk;
using Okta.Sdk.Configuration;

namespace DriveCentric.RestApi.Authorization
{
    public class GroupsToRolesTransformer : IClaimsTransformation
    {
        private OktaClient client;

        public GroupsToRolesTransformer(IConfiguration configuration)
        {
            client = new OktaClient(new OktaClientConfiguration
            {
                OrgUrl = configuration.GetSection("Okta:OrgUrl").Value.ToString(),
                Token = configuration.GetSection("Okta:Token").Value.ToString()
        });
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = principal.Identities.FirstOrDefault(x => x.IsAuthenticated);
            if (identity == null) return principal;

             
            //if (user == null) return principal;


            var idClaim = principal.FindFirst(x => x.Type == ClaimTypes.NameIdentifier);
            if (idClaim != null)
            {
                var user = await client.Users.GetUserAsync(idClaim.Value);
                if (user != null)
                {
                    var groups = user.Groups.ToEnumerable();
                    foreach (var group in groups)
                    {
                        ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(ClaimTypes.Role, group.Profile.Name));
                    }
                    var claims = new List<Claim>();
                    claims.Add(new Claim("Demeanor", "SuperCool"));
                    claims.Add(new Claim("Appearance", "Hansome"));
                    identity.AddClaims(claims);
                }
            }
            return new ClaimsPrincipal(identity);
        }
    }
}
