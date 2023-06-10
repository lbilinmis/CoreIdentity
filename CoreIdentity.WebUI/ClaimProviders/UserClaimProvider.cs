using CoreIdentity.WebUI.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CoreIdentity.WebUI.ClaimProviders
{
    public class UserClaimProvider : IClaimsTransformation
    {
        private readonly UserManager<AppUser> _userManager;

        public UserClaimProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = principal.Identity as ClaimsIdentity; // kullanıcının cookie deki kimlik bilgileri
            var currentUser = await _userManager.FindByNameAsync(identity.Name);

            if (currentUser==null)
            {
                return principal;
            }

            if (currentUser.City==null)
            {
                return principal;
            }

            
                if (principal.HasClaim(x => x.Type != "City"))
                {
                    Claim cityClaim = new Claim("City", currentUser.City.Trim());
                    identity.AddClaim(cityClaim);
                }
           
            return principal;
        }
    }
}
