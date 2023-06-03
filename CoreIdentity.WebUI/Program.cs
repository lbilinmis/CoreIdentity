using CoreIdentity.WebUI.DataAccess.EntityFramework;
using CoreIdentity.WebUI.Entities;
using CoreIdentity.WebUI.Extensions;
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
    //    opt.Password.RequireNonAlphanumeric = false; //< > * - zorunlu olmasýn
    //    opt.Password.RequireLowercase = true; //küçük harf zorunlu
    //    opt.Password.RequireUppercase = false; // büyük zorunlu deðil
    //    opt.Password.RequireDigit = false; // numeric zorunlu olmasýn

    //}).AddEntityFrameworkStores<AppDbContext>();

    builder.Services.AddIdentityWithExtension();
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