using CoreIdentity.WebUI.Common;
using Microsoft.AspNetCore.Authorization;

namespace CoreIdentity.WebUI.Requirements
{
    public class ExchangeExpireRequirement : IAuthorizationRequirement
    {
    }

    public class ExchangeExpirationRequirementHandler : AuthorizationHandler<ExchangeExpireRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExchangeExpireRequirement requirement)
        {
            var hasExchangeExpireClaim = context.User.HasClaim(x => x.Type == Constants.ClaimExchange);

            if (!hasExchangeExpireClaim)
            {
                context.Fail();
                return Task.CompletedTask;

            }

            var hasExchangeExpireDate = context.User.FindFirst(Constants.ClaimExchange);

            if (DateTime.Now > Convert.ToDateTime(hasExchangeExpireDate.Value))
            {
                context.Fail();
                return Task.CompletedTask;

            }

            context.Succeed(requirement);
            return Task.CompletedTask;

        }
    }
}
