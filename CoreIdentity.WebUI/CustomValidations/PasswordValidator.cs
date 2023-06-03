using CoreIdentity.WebUI.Entities;
using Microsoft.AspNetCore.Identity;

namespace CoreIdentity.WebUI.CustomValidations
{
    public class PasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            //password! ,user.UserName! nullable beklenmiyor

            var errors = new List<IdentityError>();

            if (password!.ToLower().Contains(user.UserName!.ToLower()))
            {
                errors.Add(new IdentityError() { Code = "PasswordContainUserName", Description = "Şifre alanı kullanıcı adı barındıramaz" });

            }

            if (password!.ToLower().StartsWith("1234"))
            {
                errors.Add(new IdentityError() { Code = "PasswordContain1234", Description = "Şifre alanı ardışık sayı barındıramaz" });

            }

            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

            return Task.FromResult(IdentityResult.Success);

        }
    }
}
