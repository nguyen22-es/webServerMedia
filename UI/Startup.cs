

using Common.DTOModels.UI;
using Common.Entities;
using Database.Contexts;
using Database.Migrations;
using Database.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false; // Tắt tính năng Endpoint Routing
            });

            services.AddDbContext<VODContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<VODUser>()
                .AddDefaultUI()
                .AddEntityFrameworkStores<VODContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddHttpClient();
            services.AddScoped<IDbReadService, DbReadService>();
            services.AddScoped<IUIReadService, UIReadService>();

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Media, MediaDTO>()
                .ForMember(dest => dest.titleModul, src => src.MapFrom(s => s.Module.Title));



        cfg.CreateMap<TopicType, TopicTypeDotDTO>()
                    .ForMember(dest => dest.TopicTypeName, src => src.MapFrom(s => s.Name))
                    .ForMember(dest => dest.TopicTypeDescription, src => src.MapFrom(s => s.Description))
                    .ForMember(dest => dest.TopicTypeAvatar, src => src.MapFrom(s => s.Thumbnail));

                cfg.CreateMap<Download, DownloadDTO>()
                    .ForMember(dest => dest.DownloadUrl, src => src.MapFrom(s => s.Url))
                    .ForMember(dest => dest.DownloadTitle, src => src.MapFrom(s => s.Title));

                cfg.CreateMap<Topic, TopicDTO>()
                    .ForMember(dest => dest.TopicId, src => src.MapFrom(s => s.Id))
                    .ForMember(dest => dest.TopicTitle, src => src.MapFrom(s => s.Title))
                    .ForMember(dest => dest.TopicDescription, src => src.MapFrom(s => s.Description))
                    .ForMember(dest => dest.MarqueeImageUrl, src => src.MapFrom(s => s.MarqueeImageUrl))
                    .ForMember(dest => dest.TopicImageUrl, src => src.MapFrom(s => s.ImageUrl));

                cfg.CreateMap<Module, ModuleDTO>()
                    .ForMember(dest => dest.ModuleTitle, src => src.MapFrom(s => s.Title));
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.Extensions.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
       
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        

        }
    }
}
