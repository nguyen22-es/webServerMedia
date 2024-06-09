using Admin;
using Common.Entities;
using Common.Services;
using Database.Contexts;
using Database.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public static class StartupHelpers
{
    public static void Startup(this IServiceCollection services, IConfiguration configuration)
    {

        
        services.AddRazorPages();
        services.AddDbContext<VODContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddDefaultIdentity<VODUser>()
            .AddRoles<IdentityRole>()
            .AddDefaultUI()
            .AddEntityFrameworkStores<VODContext>();

        
        services.AddMvc(options =>
        {
            options.EnableEndpointRouting = false; // Tắt tính năng Endpoint Routing
        });
        services.AddHttpClient("AdminClient", client =>
        {
            client.BaseAddress = new Uri("http://localhost:6600");
            client.Timeout = new TimeSpan(0, 0, 30);
            client.DefaultRequestHeaders.Clear();
        }).ConfigurePrimaryHttpMessageHandler(handler =>
            new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip
            });


        services.AddAutoMapper(typeof(StartupHelpers), typeof(TopicType), typeof(Topic), typeof(Module), typeof(Media), typeof(Download));
        services.AddScoped<IDbReadService, DbReadService>();
        services.AddScoped<IDbWriteService, DbWriteService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAdminService, AdminEFService>();
        services.AddScoped<GoogleService>();


    }
}