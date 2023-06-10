using CoreIdentity.WebUI.ClaimProviders;
using CoreIdentity.WebUI.Common;
using CoreIdentity.WebUI.DataAccess.EntityFramework;
using CoreIdentity.WebUI.Extensions;
using CoreIdentity.WebUI.OptionsModels;
using CoreIdentity.WebUI.Requirements;
using CoreIdentity.WebUI.Services.Abstract;
using CoreIdentity.WebUI.Services.Concrete;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

{
    // Add services to the container.
    builder.Services.AddControllersWithViews();

    string sqlConnection = builder.Configuration.GetConnectionString("SqlConnection");
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(sqlConnection);
    });

    #region Extensiona Taþýnanlar
    //builder.Services.AddIdentity<AppUser, AppRole>(opt =>
    //{
    //    opt.User.RequireUniqueEmail = true;// Email uniq olsun
    //    opt.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuwxyz0123456789_";

    //    opt.Password.RequiredLength = 6; // 6 karkter olsun
    //    opt.Password.RequireNonAlphanumeric = false; //< > * - zorunlu olmasýn
    //    opt.Password.RequireLowercase = true; //küçük harf zorunlu
    //    opt.Password.RequireUppercase = false; // büyük zorunlu deðil
    //    opt.Password.RequireDigit = false; // numeric zorunlu olmasýn

    //}).AddEntityFrameworkStores<AppDbContext>();


    //builder.Services.ConfigureApplicationCookie(opt =>
    //{

    //    var cookieBuilder = new CookieBuilder();
    //    cookieBuilder.Name = "IdentityCookie";
    //    opt.LoginPath = new PathString("/Home/SignIn");
    //    opt.LogoutPath= new PathString("/Member/LogOut2");
    //    opt.Cookie = cookieBuilder;
    //    opt.ExpireTimeSpan = TimeSpan.FromDays(60); // 60 gün boyunca cookie de tutar
    //    opt.SlidingExpiration = true;

    //});

    //builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
    //{
    //    options.TokenLifespan = TimeSpan.FromHours(2);
    //});
    #endregion

    builder.Services.AddIdentityWithExtension();
    builder.Services.AddCookieWithExtension();
    builder.Services.AddTokenWithExtension();

    builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

    builder.Services.AddScoped<IEmailService, EmailService>();
    builder.Services.Configure<SecurityStampValidatorOptions>(option =>
    {
        option.ValidationInterval = TimeSpan.FromMinutes(30);
        //30 dk da bir sunucudaki Security Stamp deðeri ile cooki de ki Security Stamp deðeri karþýlaþtýracak

    });

    builder.Services.
        AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

    builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>(); // claim iþlemleri
    builder.Services.AddScoped<IAuthorizationHandler, ExchangeExpirationRequirementHandler>(); // claim iþlemleri


    //City deðeri Diyarbakýr ya da Ýstanbul ise ilgili sayfaua eriþim saðlanýlýr
    builder.Services.AddAuthorization(opt =>
    {
        opt.AddPolicy("DiyarbakirPolicy", policy =>
        {
            policy.RequireClaim("City", "Diyarbakýr");
         
        });


        opt.AddPolicy(Constants.PolicyExchange, policy =>
        {
            policy.AddRequirements(new ExchangeExpireRequirement());

        });

    });

}


var app = builder.Build();

{

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    // bu iki middleware olmadan authentication ve authorizatioon iþlemleri yapýlamayacaktýr.
    app.UseAuthentication();

    app.UseAuthorization();


    app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();

}