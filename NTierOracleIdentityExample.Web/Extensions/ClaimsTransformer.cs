using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NTierOracleIdentityExample.Dll.Entities;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NTierOracleIdentityExample.Web.Extensions
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        #region Fields
        private UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        #endregion

        #region Constructor
        public ClaimsTransformer(UserManager<ApplicationUser> userManager, ILogger<ClaimsTransformer> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        #endregion

        #region Methods

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var id = ((ClaimsIdentity)principal.Identity);
            try
            {
                if (id != null && id.Claims != null && id.NameClaimType != null && id.RoleClaimType != null)
                {
                    var ci = new ClaimsIdentity(id.Claims, "Kerberos", id.NameClaimType, id.RoleClaimType);

                    if (ci != null)
                    {
                        var user = await _userManager.FindByNameAsync(id.Name);
                        if (user != null && (!user.LockoutEnabled || !user.LockoutEnd.HasValue || user.LockoutEnd.Value < DateTime.Now))
                        {
                            ci.AddClaim(new Claim(ClaimTypes.GivenName, user.FirstName + " " + user.LastName));
                            var roles = await _userManager.GetRolesAsync(user);
                            foreach (var item in roles)
                            {
                                ci.AddClaim(new Claim(ClaimTypes.Role, item));
                            }
                        }

                        var cp = new ClaimsPrincipal(ci);
                        return await Task.FromResult(cp);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("ERROR ON CLAIMS TRANSFORMATION: " + e.InnerException + "\n" + e.Message);
            }

            return principal;
        }

        #endregion
    }
}
