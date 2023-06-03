using CoreIdentity.WebUI.CustomValidations;
using CoreIdentity.WebUI.DataAccess.EntityFramework;
using CoreIdentity.WebUI.Entities;
using CoreIdentity.WebUI.Localizations;

namespace CoreIdentity.WebUI.Extensions
{
    public static class StartupExtension
    {
        public static void AddIdentityWithExtension(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;// Email uniq olsun
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuwxyz0123456789_";

                opt.Password.RequiredLength = 6; // 6 karkter olsun
                opt.Password.RequireNonAlphanumeric = false; //< > * - zorunlu olmasın
                opt.Password.RequireLowercase = true; //küçük harf zorunlu
                opt.Password.RequireUppercase = false; // büyük zorunlu değil
                opt.Password.RequireDigit = false; // numeric zorunlu olmasın

                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3); // dk boyunca kitle dedik
                opt.Lockout.MaxFailedAccessAttempts = 3; // 3 kaz hatalı giriş yapıldığında kitle dedik

            })

                .AddPasswordValidator<PasswordValidator>()
                .AddUserValidator<UserValidator>()
                .AddErrorDescriber<LocalizationIdentityErrorDescriber>()
                .AddEntityFrameworkStores<AppDbContext>();

            //Coookie işlemleri
            services.ConfigureApplicationCookie(opt =>
            {

                var cookieBuilder = new CookieBuilder();
                cookieBuilder.Name = "IdentityCookie";
                opt.LoginPath = new PathString("/Home/SignIn");

                opt.Cookie = cookieBuilder;
                opt.ExpireTimeSpan = TimeSpan.FromDays(60); // 60 gün boyunca cookie de tutar
                opt.SlidingExpiration = true;

            });
        }


        //public static void AddCookieWithExtension(this IServiceCollection services)
        //{
            
        //    //Coookie işlemleri
        //    services.ConfigureApplicationCookie(opt =>
        //    {

        //        var cookieBuilder = new CookieBuilder();
        //        cookieBuilder.Name = "IdentityCookie";
        //        opt.LoginPath = new PathString("/Home/SignIn");

        //        opt.Cookie = cookieBuilder;
        //        opt.ExpireTimeSpan = TimeSpan.FromDays(60); // 60 gün boyunca cookie de tutar
        //        opt.SlidingExpiration = true;

        //    });
        //}

    }
}

