using CoreIdentity.WebUI.ClaimProviders;
using CoreIdentity.WebUI.Common;
using CoreIdentity.WebUI.DataAccess.EntityFramework;
using CoreIdentity.WebUI.Entities;
using CoreIdentity.WebUI.Extensions;
using CoreIdentity.WebUI.OptionsModels;
using CoreIdentity.WebUI.Requirements;
using CoreIdentity.WebUI.Seeds;
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

    #region Extensiona Ta��nanlar
    //builder.Services.AddIdentity<AppUser, AppRole>(opt =>
    //{
    //    opt.User.RequireUniqueEmail = true;// Email uniq olsun
    //    opt.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuwxyz0123456789_";

    //    opt.Password.RequiredLength = 6; // 6 karkter olsun
    //    opt.Password.RequireNonAlphanumeric = false; //< > * - zorunlu olmas�n
    //    opt.Password.RequireLowercase = true; //k���k harf zorunlu
    //    opt.Password.RequireUppercase = false; // b�y�k zorunlu de�il
    //    opt.Password.RequireDigit = false; // numeric zorunlu olmas�n

    //}).AddEntityFrameworkStores<AppDbContext>();


    //builder.Services.ConfigureApplicationCookie(opt =>
    //{

    //    var cookieBuilder = new CookieBuilder();
    //    cookieBuilder.Name = "IdentityCookie";
    //    opt.LoginPath = new PathString("/Home/SignIn");
    //    opt.LogoutPath= new PathString("/Member/LogOut2");
    //    opt.Cookie = cookieBuilder;
    //    opt.ExpireTimeSpan = TimeSpan.FromDays(60); // 60 g�n boyunca cookie de tutar
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
        //30 dk da bir sunucudaki Security Stamp de�eri ile cooki de ki Security Stamp de�eri kar��la�t�racak

    });

    builder.Services.
        AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

    builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>(); // claim i�lemleri
    builder.Services.AddScoped<IAuthorizationHandler, ExchangeExpirationRequirementHandler>(); // claim i�lemleri
    builder.Services.AddScoped<IAuthorizationHandler, ViolenceRequirementHandler>(); // claim i�lemleri


    //City de�eri Diyarbak�r ya da �stanbul ise ilgili sayfaua eri�im sa�lan�l�r
    builder.Services.AddAuthorization(opt =>
    {
        opt.AddPolicy("DiyarbakirPolicy", policy =>
        {
            policy.RequireClaim("City", "Diyarbak�r");
        });

        opt.AddPolicy(Constants.PolicyExchange, policy =>
        {
            policy.AddRequirements(new ExchangeExpireRequirement());
        });

        opt.AddPolicy(Constants.PolicyViolence, policy =>
        {
            policy.AddRequirements(new ViolenceRequirement() { ThresholdAge = 18 });
        });

    });




}


var app = builder.Build();

{
    using (var scope = app.Services.CreateScope())
    {
        var rolemanager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
        await PermissionSeed.Seed(rolemanager);

    }

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

    // bu iki middleware olmadan authentication ve authorizatioon i�lemleri yap�lamayacakt�r.
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