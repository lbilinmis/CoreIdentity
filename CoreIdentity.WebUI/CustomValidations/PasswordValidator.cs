using CoreIdentity.WebUI.Entities;
using Microsoft.AspNetCore.Identity;

namespace CoreIdentity.WebUI.CustomValidations
{
    public class PasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            //password! ,user.UserName! nullable beklenmiyor

            var erros = new List<IdentityError>();

            if (password!.ToLower().Contains(user.UserName!.ToLower()))
            {
                erros.Add(new IdentityError() { Code = "PasswordNoContainUserName", Description = "Şifre alanı kullanıcı adı barındıramaz" });

            }

            if (password!.ToLower().StartsWith("1234"))
            {
                erros.Add(new IdentityError() { Code = "PasswordNoContain1234", Description = "Şifre alanı ardışık sayı barındıramaz" });

            }

            if (erros.Any())
            {
                return Task.FromResult(IdentityResult.Failed(erros.ToArray()));
            }

            return Task.FromResult(IdentityResult.Success);

        }
    }
}
