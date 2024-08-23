using Foodota.Core.Consts;
using Microsoft.AspNetCore.Identity;

namespace Foodota.Seeds;

public class DefaultUsers
{
    public static async Task SeedAdminUsersAsync(UserManager<ApplicationUser> userManager)
    {
        var admin = new ApplicationUser
        {
            FullName = "Admin",
            UserName = "Admin",
            Email = "Admin@gmail.com",
            EmailConfirmed = true,
        };

        var user=await userManager.FindByEmailAsync(admin.Email);
        if (user is null)
        {
            await userManager.CreateAsync(admin,"Admin@123");
            await userManager.AddToRoleAsync(admin, AppRoles.Admin);
        }
    }
}
