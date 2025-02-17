﻿using CoreIdentity.WebUI.Entities;
using Microsoft.AspNetCore.Identity;

namespace CoreIdentity.WebUI.CustomValidations
{
    public class UserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            var errors = new List<IdentityError>();
            var isNumeric = int.TryParse(user!.UserName[0].ToString(), out _);

            if (isNumeric)
            {
                errors.Add(new IdentityError() { Code = "UserNameContainFirstLetterDigit", Description = "Kullanıcı Adı ilk karakter sayısal karakter barındıramaz" });
            }

            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
