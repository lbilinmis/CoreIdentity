using CoreIdentity.WebUI.Common;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CoreIdentity.WebUI.Requirements
{
    public class ViolenceRequirement : IAuthorizationRequirement
    {
        public int ThresholdAge { get; set; }
    }

    public class ViolenceRequirementHandler : AuthorizationHandler<ViolenceRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViolenceRequirement requirement)
        {
            var hasExchangeExpireClaim = context.User.HasClaim(x => x.Type == Constants.ClaimBirthDate);

            if (!hasExchangeExpireClaim)
            {
                context.Fail();
                return Task.CompletedTask;

            }

            Claim birthDateClaim = context.User.FindFirst(Constants.ClaimBirthDate);

            var todayDate = DateTime.Now;
            var birthDate = Convert.ToDateTime(birthDateClaim.Value);

            var age = todayDate.Year - birthDate.Year;

            if (birthDate > todayDate.AddYears(-age)) age--;


            if (requirement.ThresholdAge > age)
            {
                context.Fail();
                return Task.CompletedTask;

            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }


}
