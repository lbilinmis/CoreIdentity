using CoreIdentity.WebUI.Common;
using CoreIdentity.WebUI.DataAccess.EntityFramework;
using CoreIdentity.WebUI.Entities;
using CoreIdentity.WebUI.Permissions;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CoreIdentity.WebUI.Seeds
{
    public class PermissionSeed
    {
        public static async Task Seed(RoleManager<AppRole> roleManager)
        {
            var hasBasicRole = await roleManager.RoleExistsAsync(Constants.RoleBasic);
            var hasAdvancedRole= await roleManager.RoleExistsAsync(Constants.RoleAdvanced);
            var hasAdminRole = await roleManager.RoleExistsAsync(Constants.RoleAdmin);

            if (!hasBasicRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = Constants.RoleBasic });

                var basicRole = await roleManager.FindByNameAsync(Constants.RoleBasic);


                await AddReadRolePermission(basicRole, roleManager);
            }


            if (!hasAdvancedRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = Constants.RoleAdvanced });

                var advancedRole = await roleManager.FindByNameAsync(Constants.RoleAdvanced);


                await AddReadRolePermission(advancedRole, roleManager);
                await AddReadRolePermission(advancedRole, roleManager);
            }


            if (!hasAdminRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = Constants.RoleAdmin });

                var adminRole = await roleManager.FindByNameAsync(Constants.RoleAdmin);

                await AddReadRolePermission(adminRole, roleManager);
                await AddReadRolePermission(adminRole, roleManager);
                await AddReadRolePermission(adminRole, roleManager);
            }
        }

        public static async Task AddReadRolePermission(AppRole role,RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(role, new Claim(Constants.Permission, Permission.Stock.Read));
            await roleManager.AddClaimAsync(role, new Claim(Constants.Permission, Permission.Order.Read));
            await roleManager.AddClaimAsync(role, new Claim(Constants.Permission, Permission.Catalog.Read));

        }

        public static async Task AddUpdateAndCreateRolePermission(AppRole role, RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(role, new Claim(Constants.Permission, Permission.Stock.Create));
            await roleManager.AddClaimAsync(role, new Claim(Constants.Permission, Permission.Order.Create));
            await roleManager.AddClaimAsync(role, new Claim(Constants.Permission, Permission.Catalog.Create));


            await roleManager.AddClaimAsync(role, new Claim(Constants.Permission, Permission.Stock.Update));
            await roleManager.AddClaimAsync(role, new Claim(Constants.Permission, Permission.Order.Update));
            await roleManager.AddClaimAsync(role, new Claim(Constants.Permission, Permission.Catalog.Update));

        }

        public static async Task AddDeleteRolePermission(AppRole role, RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(role, new Claim(Constants.Permission, Permission.Stock.Delete));
            await roleManager.AddClaimAsync(role, new Claim(Constants.Permission, Permission.Order.Delete));
            await roleManager.AddClaimAsync(role, new Claim(Constants.Permission, Permission.Catalog.Delete));

        }

    }
}
