using CoreIdentity.WebUI.DataAccess.EntityFramework;
using CoreIdentity.WebUI.Entities;
using CoreIdentity.WebUI.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

{
    // Add services to the container.
    builder.Services.AddControllersWithViews();

    string sqlConnection = builder.Configuration.GetConnectionString("SqlConnection");
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(sqlConnection);
    });



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

    builder.Services.AddIdentityWithExtension();
    builder.Services.AddCookieWithExtension();
    builder.Services.AddTokenWithExtension();

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