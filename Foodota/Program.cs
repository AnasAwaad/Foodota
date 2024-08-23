using Foodota.Areas.Admin.Data;
using Foodota.Mapping;
using Foodota.Seeds;
using Foodota.Services;
using Foodota.Settings;
using Foodota.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using UoN.ExpressiveAnnotations.NetCore.DependencyInjection;

namespace Foodota;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();


        builder.Services.AddControllersWithViews();



		builder.Services.AddAutoMapper(typeof(DomainProfile));
        builder.Services.AddTransient<IImageService, ImageService>();
        builder.Services.AddTransient<IEmailSender, EmailSender>();
		builder.Services.AddExpressiveAnnotations();

		builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));

		var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
        using var scope=scopeFactory.CreateScope();

        var roleManager=scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager=scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await DefaultRoles.SeedRolesAsync(roleManager);
        await DefaultUsers.SeedAdminUsersAsync(userManager);


        app.MapControllerRoute(
         name: "areas",
         pattern: "{area=exists}/{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapRazorPages();


        app.Run();
    }
}
